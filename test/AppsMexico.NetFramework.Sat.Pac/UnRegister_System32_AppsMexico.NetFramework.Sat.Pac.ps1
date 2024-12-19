# Define paths
$RegasmPath = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regasm.exe"
$DllPath = "C:\Windows\System32\AppsMexico.NetFramework.Sat.Pac.dll"

# Unregister
& $RegasmPath $DllPath /unregister

# Register
& $RegasmPath $DllPath /codebase
