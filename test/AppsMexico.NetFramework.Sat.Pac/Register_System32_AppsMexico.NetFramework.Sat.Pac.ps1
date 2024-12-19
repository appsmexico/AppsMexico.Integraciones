# Define source and destination folders
$SourceFolder = "C:\Setup\AppsMexico.NetFramework.Sat.Pac"
$DestinationFolder = "C:\Windows\System32"

# Ensure destination folder exists
if (-Not (Test-Path -Path $DestinationFolder)) {
    Write-Host "Creating destination folder: $DestinationFolder"
    New-Item -Path $DestinationFolder -ItemType Directory -Force
}

# Path to the DLL in the destination folder
$DllPath = Join-Path -Path $DestinationFolder -ChildPath "AppsMexico.NetFramework.Sat.Pac.dll"

# Path to regasm
$RegasmPath = Join-Path -Path $env:SystemRoot -ChildPath "Microsoft.NET\Framework\v4.0.30319\regasm.exe"

# Path to gacutil
$GacutilPath = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\gacutil.exe"

# Unregister the existing DLL if it exists
if (Test-Path -Path $DllPath) {
    Write-Host "Unregistering existing DLL..."
    if (Test-Path -Path $RegasmPath) {
        & $RegasmPath $DllPath /u
        Write-Host "Existing DLL unregistered successfully."
    } else {
        Write-Host "ERROR: regasm.exe not found at $RegasmPath" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "No existing DLL found to unregister."
}

# Copy all content from the source folder to the destination folder
Write-Host "Copying files from $SourceFolder to $DestinationFolder..."
Copy-Item -Path "$SourceFolder\*" -Destination $DestinationFolder -Recurse -Force

# Validate that the DLL exists in the destination folder
if (-Not (Test-Path -Path $DllPath)) {
    Write-Host "ERROR: DLL not found at $DllPath" -ForegroundColor Red
    exit 1
}

# Register the DLL
Write-Host "Registering DLL with regasm..."
if (Test-Path -Path $RegasmPath) {
    & $RegasmPath $DllPath /codebase
    Write-Host "DLL registered successfully."
} else {
    Write-Host "ERROR: regasm.exe not found at $RegasmPath" -ForegroundColor Red
    exit 1
}

# Add the DLL to the GAC
Write-Host "Adding DLL to GAC with gacutil..."
if (Test-Path -Path $GacutilPath) {
    & $GacutilPath -i $DllPath
    Write-Host "DLL added to GAC successfully."
} else {
    Write-Host "ERROR: gacutil.exe not found. Ensure the Windows SDK is installed." -ForegroundColor Red
    exit 1
}

Write-Host "Process completed successfully." -ForegroundColor Green
