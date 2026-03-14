param(
    [Parameter(Mandatory = $true)]
    [string]$TargetMigration
)

# Requirement Traceability: MC-006, NFR-004
# Evidence notes: capture migration target, UTC execution timestamp, operator, and post-rollback health check output.

$ErrorActionPreference = 'Stop'

Write-Host "Rolling database back to migration: $TargetMigration"
dotnet ef database update $TargetMigration --project InTimeProAPI/InTimeProAPI.csproj
if ($LASTEXITCODE -ne 0) {
    throw "Rollback failed"
}

Write-Host "Rollback completed successfully"

$evidenceDir = Join-Path $PSScriptRoot '..\..\artifacts'
New-Item -ItemType Directory -Path $evidenceDir -Force | Out-Null
$evidencePath = Join-Path $evidenceDir 'rollback-evidence.json'

$evidence = [ordered]@{
    requirementIds = @('MC-006', 'NFR-004')
    targetMigration = $TargetMigration
    executedAtUtc = (Get-Date).ToUniversalTime().ToString('o')
    operator = $env:USERNAME
    status = 'success'
}

$evidence | ConvertTo-Json -Depth 4 | Set-Content $evidencePath
Write-Host "Rollback evidence written to: $evidencePath"
