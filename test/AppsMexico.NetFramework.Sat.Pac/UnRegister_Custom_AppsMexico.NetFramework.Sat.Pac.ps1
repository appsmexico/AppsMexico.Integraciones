# Define paths
$RegasmPath = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regasm.exe"
$DllPath = "C:\Setup\AppsMexico.NetFramework.Sat.Pac\Release\AppsMexico.NetFramework.Sat.Pac.dll"

# Unregister
& $RegasmPath $DllPath /unregister

# Register
& $RegasmPath $DllPath /codebase
