[CmdletBinding()]
param(
    [string]$ManifestPath = "build/example-build-manifest.json"
)

$ErrorActionPreference = "Stop"
$allowedCategories = @("api-styles", "authentication", "architecture", "integration", "infrastructure", "user-interface")
$categorySolutions = @{
    "api-styles" = "solutions/zenvera.api-styles.slnx"
    "authentication" = "solutions/zenvera.authentication.slnx"
    "architecture" = "solutions/zenvera.architecture.slnx"
    "integration" = "solutions/zenvera.integration.slnx"
    "infrastructure" = "solutions/zenvera.infrastructure.slnx"
    "user-interface" = "solutions/zenvera.user-interface.slnx"
}
$manifest = Get-Content -Raw -LiteralPath $ManifestPath | ConvertFrom-Json

if ($manifest.schemaVersion -ne 1) { throw "Unsupported manifest schemaVersion '$($manifest.schemaVersion)'." }
if ($manifest.examples.Count -ne 31) { throw "Expected 31 catalog examples, found $($manifest.examples.Count)." }

$duplicateIds = $manifest.examples | Group-Object id | Where-Object Count -gt 1
if ($duplicateIds) { throw "Duplicate example ids: $($duplicateIds.Name -join ', ')" }

foreach ($example in $manifest.examples) {
    foreach ($field in @("id", "category", "path", "framework", "buildEnabled", "testEnabled", "externalDependencies")) {
        if ($null -eq $example.$field -or ($example.$field -is [string] -and [string]::IsNullOrWhiteSpace($example.$field))) {
            throw "Example '$($example.id)' is missing required field '$field'."
        }
    }
    if ($example.category -notin $allowedCategories) { throw "Example '$($example.id)' has unknown category '$($example.category)'." }
    if (-not (Test-Path -LiteralPath $example.path -PathType Leaf)) { throw "Example '$($example.id)' path does not exist: $($example.path)" }
    if (-not $example.buildEnabled -and [string]::IsNullOrWhiteSpace($example.exclusionReason)) {
        throw "Disabled example '$($example.id)' must have an exclusionReason."
    }
    if ($example.buildEnabled -and -not [string]::IsNullOrWhiteSpace($example.exclusionReason)) {
        throw "Enabled example '$($example.id)' must not have an exclusionReason."
    }
    if ($example.testEnabled) {
        if (-not $example.buildEnabled) { throw "Test-enabled example '$($example.id)' must also be build-enabled." }
        if ([string]::IsNullOrWhiteSpace($example.testPath) -or -not (Test-Path -LiteralPath $example.testPath -PathType Leaf)) {
            throw "Test-enabled example '$($example.id)' needs an existing testPath."
        }
    }
}

foreach ($category in $allowedCategories) {
    if (-not ($manifest.examples.category -contains $category)) { throw "Manifest category '$category' has no examples." }
    $solutionPath = $categorySolutions[$category]
    if (-not (Test-Path -LiteralPath $solutionPath -PathType Leaf)) { throw "Category solution is missing: $solutionPath" }
    [xml]$solution = Get-Content -Raw -LiteralPath $solutionPath
    $solutionDirectory = Split-Path -Parent (Resolve-Path $solutionPath)
    $solutionProjects = @($solution.Solution.Folder.Project.Path) | ForEach-Object {
        [System.IO.Path]::GetRelativePath((Resolve-Path ".").Path, [System.IO.Path]::GetFullPath((Join-Path $solutionDirectory $_))).Replace('\','/')
    }
    foreach ($example in $manifest.examples | Where-Object category -eq $category) {
        if ($example.path.Replace('\','/') -notin $solutionProjects) {
            throw "Example '$($example.id)' is missing from category solution '$solutionPath'."
        }
    }
}

Write-Host "Validated $($manifest.examples.Count) examples across $($allowedCategories.Count) categories."
foreach ($excluded in $manifest.examples | Where-Object { -not $_.buildEnabled }) {
    Write-Host "EXCLUDED $($excluded.id): $($excluded.exclusionReason)"
}
