$ErrorActionPreference = 'Stop'

$configPath = Join-Path $PSScriptRoot '..\..\appsettings.json'
$configPath = [System.IO.Path]::GetFullPath($configPath)
$config = Get-Content $configPath -Raw | ConvertFrom-Json
$conn = $config.ConnectionStrings.DefaultConnection

$checks = @(
    @{ Name = 'Encrypt'; Pass = $conn -match 'Encrypt=True' },
    @{ Name = 'TrustServerCertificate'; Pass = $conn -match 'TrustServerCertificate=False' },
    @{ Name = 'MaxPoolSize'; Pass = $conn -match 'Max Pool Size=' }
)

$result = [ordered]@{
    timestamp = (Get-Date).ToString('o')
    passed = ($checks | Where-Object { -not $_.Pass }).Count -eq 0
    checks = $checks
}

$outDir = Join-Path $PSScriptRoot '..\..\artifacts'
New-Item -ItemType Directory -Path $outDir -Force | Out-Null
$outFile = Join-Path $outDir 'sql-connection-gate.json'
$result | ConvertTo-Json -Depth 5 | Set-Content $outFile

if (-not $result.passed) {
    Write-Error "Connection validation failed. See $outFile"
    exit 1
}

Write-Host "Connection validation passed. Artifact: $outFile"
