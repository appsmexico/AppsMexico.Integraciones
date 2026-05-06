using FacturacionElectronica.Net.Xml.Model.Account;
using FacturacionElectronica.Net.Xml.Model.Comprobante;
using FacturacionElectronica.Net.Xml.Services;
using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

IConfigurationSection appSettings = configuration.GetSection("AppSettings");

// Variables de configuración
string directorioArchivosPrueba = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\..\test\FE\4.0"));
string empresaId = appSettings["EmpresaId"]!;
bool modoDebug = appSettings.GetValue<bool>("ModoDebug");
bool modoPrueba = appSettings.GetValue<bool>("ModoPrueba");

// API endpoints
string baseUrl = appSettings["BaseApiUrl"]!;
string loginUrl = $"{baseUrl}/Account/Login";
string timbrarUrl = $"{baseUrl}/ErpComprobantes/TimbrarComprobanteErpFromXml";
string cancelarUrl = $"{baseUrl}/ErpComprobantes/CancelarComprobanteErpV2";

Login login = new()
{
    UserId = appSettings["Usuario"],
    Password = appSettings["Password"],
    EmpresaId = empresaId,
};

ApiService apiService = new(loginUrl, timbrarUrl, cancelarUrl);

Console.WriteLine("Seleccione la operación a ejecutar:");
Console.WriteLine("  1 - Timbrar CFDI");
Console.WriteLine("  2 - Cancelar CFDI Sin Relacion. Codigo 02");
Console.WriteLine("  3 - Cancelar CFDI Con Relacion. Codigo 01");
Console.Write("Opción: ");
string? opcion = Console.ReadLine()?.Trim();

switch (opcion)
{
    case "1":
        await EjecutarTimbrado();
        break;
    case "2":
        await EjecutarCancelacion(motivo: "02", uuidSustitucion: null);
        break;
    case "3":
        await EjecutarCancelacion(motivo: "01", uuidSustitucion: new Guid("FE5B60BD-C435-4656-8FF3-00F08EDB9421"));
        break;
    default:
        Console.WriteLine("Opción no válida.");
        break;
}

Console.WriteLine("\nPresione cualquier tecla para salir...");
Console.ReadKey();

#region Timbrar CFDI

/// Ejemplo de timbrado de CFDI a través de la API, utilizando un archivo XML de prueba ubicado en el proyecto.
async Task EjecutarTimbrado()
{
    try
    {
        string filePath = Path.Combine(directorioArchivosPrueba, "prueba_cfdi40_ingreso.xml");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"El archivo XML no se encontró en la ruta especificada: {filePath}", filePath);
        }

        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

        TimbradoCfdiRequest timbradoRequest = new()
        {
            Id = "F00134",
            RfcEmisor = "EKU9003173C9",
            ArchivoBase64 = Convert.ToBase64String(await File.ReadAllBytesAsync(filePath)),
            ProcesoId = "0803000",
            GenerarPdf = false,
            ModoDebug = modoDebug,
            ModoPrueba = modoPrueba,
        };

        TimbradoCfdiResponse response = await apiService.TimbrarCfdiAsync(login, timbradoRequest);
        Console.WriteLine($"Respuesta Timbrado: {response.Descripcion} - {response.Id}");

        if (response.OperacionExitosa && !string.IsNullOrWhiteSpace(response.ArchivoXmlBase64))
        {
            string rutaSalida = Path.Combine(directorioArchivosPrueba, $"{fileNameWithoutExtension}_timbrado.xml");
            await File.WriteAllBytesAsync(rutaSalida, Convert.FromBase64String(response.ArchivoXmlBase64));
            Console.WriteLine($"XML Timbrado guardado en: {rutaSalida}");

            if (timbradoRequest.GenerarPdf && !string.IsNullOrWhiteSpace(response.ArchivoPdfBase64))
            {
                rutaSalida = Path.Combine(directorioArchivosPrueba, $"{fileNameWithoutExtension}_timbrado.pdf");
                await File.WriteAllBytesAsync(rutaSalida, Convert.FromBase64String(response.ArchivoPdfBase64));
                Console.WriteLine($"PDF guardado en: {rutaSalida}");
            }
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(response.ArchivoXmlRequestBase64))
            {
                string rutaSalida = Path.Combine(directorioArchivosPrueba, $"{fileNameWithoutExtension}_request.xml");
                await File.WriteAllBytesAsync(rutaSalida, Convert.FromBase64String(response.ArchivoXmlRequestBase64));
                Console.WriteLine($"XML Request guardado en: {rutaSalida}");
            }

            if (!string.IsNullOrWhiteSpace(response.ArchivoXmlResponseBase64))
            {
                string rutaSalida = Path.Combine(directorioArchivosPrueba, $"{fileNameWithoutExtension}_response.xml");
                await File.WriteAllBytesAsync(rutaSalida, Convert.FromBase64String(response.ArchivoXmlResponseBase64));
                Console.WriteLine($"XML Response guardado en: {rutaSalida}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al timbrar: {ex.Message}");
    }
}

#endregion

#region Cancelar CFDI

/// Ejemplo de cancelación de CFDI a través de la API.
/// El motivo "02" indica cancelación sin sustitución; "01" requiere un UUID de sustitución.
/// Este ejemplo asume el CFDI con UUID "B5400F16-DB4B-49B6-AC76-9FA01B2711C3".
async Task EjecutarCancelacion(string motivo, Guid? uuidSustitucion)
{
    try
    {
        CancelarCfdiRequest cancelacionDTO = new()
        {
            EmpresaId = empresaId,
            ErpId = "F00134",
            FechaCancelado = DateTime.Now,
            MotivoCancelacion = "Prueba de cancelacion de CFDI",
            MotivoCancelacionCodigo = motivo,
            RfcReceptor = "IIA040805DZ4",
            SelloDigital = "ISfvxsElP7VrQmCSHodVKjh4uo7JReFyV+t2pXGQjpy43ba2mplHwAakOJezOqHKH7WFcdCokbwjyfTAHtJvHAe+2pz2svL2dtjGQY0/fpo7vE9ggSXEeqtYiep/hUuCabWPzTHUrLTPIsauybCe+OK/ngh/dVStogyoaTEiKLQuPMfp1XNQfygsnfxZlkKVoh1tNwzEN1mXzvf6CPsk/8+zkAtRoWqvIQT5sb64tPvod5G9qnY8eMLpXyIEohusde1MKL/OtnFtA46sN42eeWWMOAxyBEbdT6O2UtHTUlUhWwvbNGqfgS0hFMo+58eyM+SVugi0rwQGniLrgPqzKA==",
            Total = 754.00,
            UUID = new Guid("B5400F16-DB4B-49B6-AC76-9FA01B2711C3"),
            UUIDSustitucion = uuidSustitucion,
        };

        CancelarCfdiResponse response = await apiService.CancelarCfdiAsync(login, cancelacionDTO);
        Console.WriteLine($"Resultado cancelación: [{response.Id}] {response.Descripcion}");

        string prefijo = $"{DateTime.Now:yyyyMMddHHmmss}_pruebacancelacion{motivo}";

        if (response.OperacionExitosa)
        {
            if (!string.IsNullOrEmpty(response.ArchivoXmlAcuseBase64))
            {
                string rutaSalida = Path.Combine(directorioArchivosPrueba, $"{prefijo}_acuse.xml");
                await File.WriteAllBytesAsync(rutaSalida, Convert.FromBase64String(response.ArchivoXmlAcuseBase64));
                Console.WriteLine($"XML Acuse guardado en: {rutaSalida}");
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(response.ArchivoXmlRequestBase64))
            {
                string rutaSalida = Path.Combine(directorioArchivosPrueba, $"{prefijo}_request.xml");
                await File.WriteAllBytesAsync(rutaSalida, Convert.FromBase64String(response.ArchivoXmlRequestBase64));
                Console.WriteLine($"XML Request guardado en: {rutaSalida}");
            }

            if (!string.IsNullOrEmpty(response.ArchivoXmlResponseBase64))
            {
                string rutaSalida = Path.Combine(directorioArchivosPrueba, $"{prefijo}_response.xml");
                await File.WriteAllBytesAsync(rutaSalida, Convert.FromBase64String(response.ArchivoXmlResponseBase64));
                Console.WriteLine($"XML Response guardado en: {rutaSalida}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al cancelar: {ex.Message}");
    }
}

#endregion

