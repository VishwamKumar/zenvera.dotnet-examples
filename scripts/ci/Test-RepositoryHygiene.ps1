[CmdletBinding()]
param()

$ErrorActionPreference = "Stop"
$root = (Resolve-Path "$PSScriptRoot/../..").Path

# Prefer git-tracked and non-ignored files so CI/local do not require ripgrep.
$gitListing = @(git -C $root ls-files --cached --others --exclude-standard 2>$null)
if ($LASTEXITCODE -ne 0) {
    throw "git ls-files failed while enumerating repository files for hygiene checks."
}

$files = @(
    $gitListing |
        Where-Object { -not [string]::IsNullOrWhiteSpace($_) } |
        ForEach-Object {
            $normalized = $_.Replace('/', [System.IO.Path]::DirectorySeparatorChar)
            [System.IO.Path]::GetFullPath((Join-Path $root $normalized))
        } |
        Where-Object { Test-Path -LiteralPath $_ -PathType Leaf }
)
$failures = [System.Collections.Generic.List[string]]::new()

$badPathPattern = '(^|[\\/])(bin|obj|\.vs|\.idea)([\\/]|$)|\.(user|suo|db|db-shm|db-wal|sqlite|sqlite3)$'
foreach ($file in $files) {
    $relative = [System.IO.Path]::GetRelativePath($root, $file)
    if ($relative -match $badPathPattern) { $failures.Add("Forbidden generated/user file: $relative") }
}

$secretPatterns = @(
    '-----BEGIN (RSA |EC |OPENSSH )?PRIVATE KEY-----',
    'AKIA[0-9A-Z]{16}',
    'gh[pousr]_[A-Za-z0-9]{30,}',
    '(?i)(AccountKey|ClientSecret)\s*[=:]\s*["'']?(?!ENV:|replace-|<|\$\{)[A-Za-z0-9+/=_-]{16,}'
)
$textExtensions = @('.cs','.csproj','.props','.targets','.json','.jsonc','.yml','.yaml','.xml','.config','.md','.ps1','.sh','.http','.proto','.razor','.xaml')
foreach ($file in $files | Where-Object { [System.IO.Path]::GetExtension($_) -in $textExtensions }) {
    $content = Get-Content -Raw -LiteralPath $file -ErrorAction SilentlyContinue
    foreach ($pattern in $secretPatterns) {
        if ($content -match $pattern) {
            $relative = [System.IO.Path]::GetRelativePath($root, $file)
            $failures.Add("Potential committed secret ($pattern): $relative")
        }
    }
}

$markdownFiles = Get-ChildItem $root -Recurse -Filter *.md -File |
    Where-Object { $_.FullName -notmatch '[\\/](\.git|bin|obj)[\\/]' }
foreach ($file in $markdownFiles) {
    $markdown = Get-Content -Raw -LiteralPath $file.FullName
    foreach ($match in [regex]::Matches($markdown, '(?<!\!)\[[^\]]+\]\((?!https?://|#|mailto:)([^)]+)\)')) {
        $raw = $match.Groups[1].Value.Trim('<','>')
        $target = [uri]::UnescapeDataString($raw.Split('#')[0])
        if ($target -and -not (Test-Path -LiteralPath (Join-Path $file.DirectoryName $target))) {
            $relative = [System.IO.Path]::GetRelativePath($root, $file.FullName)
            $failures.Add("Broken Markdown link: $relative -> $raw")
        }
    }
}

$composeFiles = @(Get-ChildItem $root -Recurse -File |
    Where-Object {
        $_.FullName -notmatch '[\\/](\.git|bin|obj)[\\/]' -and
        $_.Name -in @('compose.yml','compose.yaml','docker-compose.yml','docker-compose.yaml')
    })
foreach ($compose in $composeFiles) {
    docker compose -f $compose.FullName config --quiet
    if ($LASTEXITCODE -ne 0) { $failures.Add("Invalid Docker Compose file: $($compose.FullName)") }
}

if ($failures.Count -gt 0) {
    $failures | ForEach-Object { Write-Error $_ }
    throw "Repository hygiene validation failed with $($failures.Count) finding(s)."
}

Write-Host "Repository hygiene passed: $($files.Count) files, $($markdownFiles.Count) Markdown files, $($composeFiles.Count) Compose files."
