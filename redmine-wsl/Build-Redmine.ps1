[CmdletBinding()]
param(
    [string]$DataPath = 'C:\redmine',
    [string]$Name = 'redmine-evm',
    [ValidateRange(1, 65535)]
    [int]$Port = 3000
)

$ErrorActionPreference = 'Stop'
$image = 'redmine-evm:wslc'

wslc build -t $image $PSScriptRoot
if ($LASTEXITCODE -ne 0) {
    throw "wslc build failed (exit code $LASTEXITCODE)."
}

$data = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($DataPath)
$sqlite = Join-Path $data 'sqlite'
$files = Join-Path $data 'files'
New-Item -ItemType Directory -Force -Path $sqlite, $files | Out-Null

$envFile = Join-Path $data 'redmine.env'
if (-not (Test-Path -LiteralPath $envFile)) {
    $bytes = [byte[]]::new(64)
    $rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
    try { $rng.GetBytes($bytes) } finally { $rng.Dispose() }
    $secret = [Convert]::ToBase64String($bytes)
    Set-Content -LiteralPath $envFile -Encoding ascii -Value "SECRET_KEY_BASE=$secret"
}

wslc run -d --name $Name -p "127.0.0.1:${Port}:3000" `
    --env-file $envFile `
    --mount "type=bind,source=$sqlite,target=/usr/src/redmine/sqlite" `
    --mount "type=bind,source=$files,target=/usr/src/redmine/files" `
    $image
if ($LASTEXITCODE -ne 0) {
    throw "wslc run failed (exit code $LASTEXITCODE)."
}
