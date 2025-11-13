param(
    [string]$PublishDir = "C:\Published\YumBlazor",
    [string]$CertThumbprint = "F00E8357FB0BAB0138AF442230B554B6AFD020E1"
)

$ErrorActionPreference = "Stop"

$signTool = "C:\Program Files (x86)\Windows Kits\10\bin\10.0.19041.0\x64\signtool.exe"

if (-not (Test-Path $signTool)) {
    Write-Error "signtool.exe not found at path: $signTool"
    exit 1
}

if (-not (Test-Path $PublishDir)) {
    Write-Error "Publish directory not found: $PublishDir"
    exit 1
}

Write-Host "Signing DLLs in: $PublishDir"
Write-Host "Using certificate thumbprint: $CertThumbprint"

$files = Get-ChildItem -Path $PublishDir -Filter *.dll

if (-not $files) {
    Write-Host "No DLLs found in $PublishDir"
    exit 0
}

foreach ($file in $files) {
    Write-Host "Signing $($file.Name)..."

    # SIGN
    & $signTool sign /sha1 $CertThumbprint /fd SHA256 $file.FullName

    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to sign $($file.Name). signtool exit code: $LASTEXITCODE"
        exit $LASTEXITCODE
    }

    # VERIFY
    $signature = Get-AuthenticodeSignature $file.FullName

    if ($signature.Status -eq 'Valid') {
        Write-Host "Verified: $($file.Name) (Valid)"
    }
    elseif ($signature.Status -eq 'NotSigned') {
        Write-Host "Warning: $($file.Name) is NOT signed."
    }
    else {
        Write-Host "Warning: $($file.Name) signature status: $($signature.Status)"
    }
}

Write-Host "All DLLs processed."
