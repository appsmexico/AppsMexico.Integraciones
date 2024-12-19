# Define the DLL directory and path
$DllDirectory = "C:\Setup\AppsMexico.NetFramework.Sat.Pac\Release"
$DllPath = Join-Path -Path $DllDirectory -ChildPath "AppsMexico.NetFramework.Sat.Pac.dll"

# Define regasm paths (32-bit and 64-bit)
$RegasmPath32 = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\regasm.exe"
$RegasmPath64 = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regasm.exe"

# Check if the DLL exists
if (-Not (Test-Path -Path $DllPath)) {
    Write-Host "ERROR: DLL not found at $DllPath" -ForegroundColor Red
    exit 1
}

# Determine regasm path based on DLL architecture
$RegasmPath = if ([Environment]::Is64BitProcess) { $RegasmPath64 } else { $RegasmPath32 }

# Validate regasm path
if (-Not (Test-Path -Path $RegasmPath)) {
    Write-Host "ERROR: regasm.exe not found at $RegasmPath" -ForegroundColor Red
    exit 1
}

# Unregister the DLL
Write-Host "Unregistering DLL..."
& $RegasmPath $DllPath /unregister
if ($LASTEXITCODE -eq 0) {
    Write-Host "DLL unregistered successfully."
} else {
    Write-Host "WARNING: Failed to unregister the DLL." -ForegroundColor Yellow
}

# Register the DLL with codebase
Write-Host "Registering DLL..."
& $RegasmPath $DllPath /codebase
if ($LASTEXITCODE -eq 0) {
    Write-Host "DLL registered successfully."
} else {
    Write-Host "ERROR: Failed to register the DLL." -ForegroundColor Red
    exit 1
}

Write-Host "Process completed successfully." -ForegroundColor Green
