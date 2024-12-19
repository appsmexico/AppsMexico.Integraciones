Dim cfdiService

' Set the current directory to the location of the DLLs
Set fso = CreateObject("Scripting.FileSystemObject")
Set shell = CreateObject("WScript.Shell")
currentDirectory = fso.GetParentFolderName(WScript.ScriptFullName)
shell.CurrentDirectory = currentDirectory

On Error Resume Next
Set cfdiService = CreateObject("AppsMexico.NetFramework.Sat.Pac.CfdiService")

If Err.Number <> 0 Then
    LogError "Error creating CfdiService object: " & Err.Description & " (Code: " & Hex(Err.Number) & ")"
    WScript.Quit(1)
End If
On Error GoTo 0

' Initialize the service
On Error Resume Next
cfdiService.Initialize "https://cloud.appsmexico.mx/am/erp/WebApiDemo/api/es-MX", True, True, "democfdi@appsmexico.mx", "Pa$$w0rdDem0", "Todos"
If Err.Number <> 0 Then
    LogError "Error initializing CfdiService: " & Err.Description & " (Code: " & Hex(Err.Number) & ")"
    WScript.Quit(1)
End If
On Error GoTo 0

' CancelarComprobanteErp usage
Dim procesoId, empresaErpId, empresaRfc, comprobanteErpId, timbrarResult
procesoId = "SAT2400"
empresaErpId = "1100"
empresaRfc = "EKU9003173C9"
comprobanteErpId = "CA8D72FC-B885-43DF-B5CF-60D3D91479FD"

' Example usage of CancelarComprobanteErp
Dim uuid, total, rfcReceptor, selloDigital, motivoCancelacionCodigo, fechaCancelacion, uuidSustitucion
uuid = "DE1AB0C8-B225-49C5-8BF9-BC4A9767AF82"
total = 8816.00
rfcReceptor = "IIA040805DZ4"
selloDigital ="lxAlvq7wADHSk4nVpFsVh+NYw343nIfgNgE7kcDI53PGliyillA8zn+w2Vg45EaInvM3eT+r1IgGaXYXPn/4Qzf6eKmNoqLmKP3iTAoRUEvwyiEmPydxVvWD/7LAxWJUkf3sokzqf1fy4b+vo9VD3+YFtTLSfLWhtBMGiSdc7hSPTLRBXQgFF2Uh0voJDXWRj7rNM5reuG6NzcsBmTLGy05rhoZ4FJQPa8zNpz0oLv0SeOU9mg1jApoHTxjMngUAPKJ0t6mOAtQP7qKE354ySEokfXEPOf8s4FW8vQkt97aK5+vGHBWK/mmioq6brsBs32jAlDtUlU48r83TuYbsNQ=="
motivoCancelacionCodigo = "02"
fechaCancelacion = Null ' Opcional, si no se especifica una fecha se utiliza la fecha y hora actual
uuidSustitucion = Null ' Condicional, es requerido cuando motivoCancelacionCodigo es "01"

On Error Resume Next
timbrarResult = cfdiService.CancelarComprobanteErpSync(empresaErpId, comprobanteErpId, uuid, total, rfcReceptor, selloDigital, motivoCancelacionCodigo, fechaCancelacion, uuidSustitucion)
If Err.Number <> 0 Then
    LogError "Error in CancelarComprobanteErp: " & Err.Description & " (Code: " & Hex(Err.Number) & ")"
    WScript.Quit(1)
End If
On Error GoTo 0

WScript.Echo "Timbrar Result: " & timbrarResult

Sub LogError(errorMessage)
    Dim fso, logFile
    Set fso = CreateObject("Scripting.FileSystemObject")
    Set logFile = fso.OpenTextFile("C:\Setup\AppsMexico.NetFramework.Sat.Pac\error.log", 8, True)
    logFile.WriteLine Now & " - " & errorMessage
    logFile.Close
End Sub
