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

' TimbrarComprobanteMyBusinessFromXmlSync usage
Dim procesoId, empresaErpId, empresaRfc, comprobanteErpId, rutaArchivoXml, archivoXmlBase64, generarPdf, timbrarResult
procesoId = "SAT2400"
empresaErpId = "1100"
empresaRfc = "EKU9003173C9"
comprobanteErpId = "CA8D72FC-B885-43DF-B5CF-60D3D91479FD"
rutaArchivoXml = "C:\Setup\AppsMexico.NetFramework.Sat.Pac\Demo_CFDI_Ingreso_IVA16.xml"
archivoXmlBase64 = ""
generarPdf = True

On Error Resume Next
timbrarResult = cfdiService.TimbrarComprobanteMyBusinessFromXmlSync(procesoId, empresaErpId, empresaRfc, comprobanteErpId, rutaArchivoXml, archivoXmlBase64, generarPdf)
If Err.Number <> 0 Then
    LogError "Error in TimbrarComprobanteMyBusinessFromXmlSync: " & Err.Description & " (Code: " & Hex(Err.Number) & ")"
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
