[CmdletBinding()]
param(
    [ValidateSet("all", "api-styles", "authentication", "architecture", "integration", "infrastructure", "user-interface")]
    [string]$Category = "all",
    [ValidateSet("restore", "build", "test", "format")]
    [string]$Action = "build"
)

$ErrorActionPreference = "Stop"
& "$PSScriptRoot/Test-ExampleBuildManifest.ps1"
$manifest = Get-Content -Raw -LiteralPath "build/example-build-manifest.json" | ConvertFrom-Json
$examples = @($manifest.examples | Where-Object { $Category -eq "all" -or $_.category -eq $Category })

foreach ($excluded in $examples | Where-Object { -not $_.buildEnabled }) {
    Write-Host "::notice title=Tracked build exclusion::$($excluded.id): $($excluded.exclusionReason)"
}

foreach ($example in $examples | Where-Object buildEnabled) {
    Write-Host "--- $($example.id) [$($example.framework)] ---"
    switch ($Action) {
        "restore" {
            dotnet restore $example.path --configfile NuGet.config -p:NuGetAudit=false -v:minimal
            if ($LASTEXITCODE -ne 0) { throw "Restore failed: $($example.id)" }
            if ($example.testEnabled) {
                dotnet restore $example.testPath --configfile NuGet.config -p:NuGetAudit=false -v:minimal
                if ($LASTEXITCODE -ne 0) { throw "Test restore failed: $($example.id)" }
            }
        }
        "build" {
            dotnet build $example.path --no-restore -p:NuGetAudit=false --configuration Release -v:minimal
            if ($LASTEXITCODE -ne 0) { throw "Build failed: $($example.id)" }
        }
        "test" {
            if ($example.testEnabled) {
                dotnet test $example.testPath --no-restore -p:NuGetAudit=false --configuration Release -v:minimal
                if ($LASTEXITCODE -ne 0) { throw "Tests failed: $($example.id)" }
            }
        }
        "format" {
            dotnet format whitespace $example.path --verify-no-changes --no-restore --verbosity minimal
            if ($LASTEXITCODE -ne 0) { throw "Formatting validation failed: $($example.id)" }
            if ($example.testEnabled) {
                dotnet format whitespace $example.testPath --verify-no-changes --no-restore --verbosity minimal
                if ($LASTEXITCODE -ne 0) { throw "Test formatting validation failed: $($example.id)" }
            }
        }
    }
}
