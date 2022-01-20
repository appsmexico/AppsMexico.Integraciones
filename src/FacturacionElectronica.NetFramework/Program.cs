//Se deben instalar estos paquetes NuGet para que el proceso se ejecute correctamente
//Agregar referncia a libreria build\AppsMexico.Common.Sat.Pac
//<package id="System.Configuration.ConfigurationManager" version="6.0.0" targetFramework="net462" />
//<package id="Microsoft.AspNetCore.Mvc.Core" version="2.2.0" targetFramework="net462" />
//<package id="Microsoft.Extensions.Localization.Abstractions" version="5.0.13" targetFramework="net462" />
//<package id="Microsoft.Extensions.Logging.Abstractions" version="5.0.0" targetFramework="net462" />
//<package id="Microsoft.Extensions.Options, Version=5.0.0.0
//<package id="Newtonsoft.Json, Version=13.0.0.0
//<package id="Serilog 2.0.0
//<package id="Serilog.Sinks.Console, Version=4.0.1.0
//<package id="Serilog.Sinks.File, Version=5.0.0.0
//<package id="System.ComponentModel.Annotations, Version=4.2.0.0
//<package id="System.ServiceModel.Primitives" version="4.8.1" targetFramework="net462" />
//<package id="System.ServiceModel.Http" version="4.8.1" targetFramework="net462" />
//<package id="System.Text.Encoding.CodePages" version="6.0.0" targetFramework="net462" />

namespace FacturacionElectronica.NetFramework
{
    using AppsMexico.Common.Classes.Sat;
    using AppsMexico.Common.Functions;
    using AppsMexico.Common.Sat.Pac;
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    internal class Program
    {
        private static bool ModoDebug { get => Convert.ToBoolean(ConfigurationManager.AppSettings["ModoDebug"]); }
        private static bool ModoPrueba { get => Convert.ToBoolean(ConfigurationManager.AppSettings["ModoPrueba"]); }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Ingrese el proceso que desea realizar. Timbrado = T o Cancelacion = C");
                string vRespuesta = Console.ReadLine();
                if (string.Compare(vRespuesta, "T", true) == 0)
                {
                    Console.WriteLine("TimbrarCfdi");
                    Task.Run(async () => await TimbrarCfdi());
                }
                else if (string.Compare(vRespuesta, "C", true) == 0)
                {
                    Console.WriteLine("CancelarCfdi");
                    Task.Run(async () => await CancelarCfdi());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ExceptionFunctions.GetExceptionMessage(ex, ModoDebug)}");
            }
            Console.WriteLine("Press ENTER to finish");
            Console.ReadLine();
        }


        #region Timbrar CFDI
        private static async Task TimbrarCfdi()
        {
            try
            {
                string vSerie = "A";
                string vFolio = "37";
                string vRfcEmisor = "XIA190128J61";

                //Para realizar el timbrado se puede proporcionar el XML en alguno de los 3 diferentes tipos de dato (Archivo o ArchivoBase64 o RutaArchivo).
                TimbradoCfdiRequest vTimbradoCfdiRequest = new TimbradoCfdiRequest()
                {
                    Archivo = null, // Condicional - Arreglo de bytes del archivo XML que se va a timbrar. 
                    ArchivoBase64 = string.Empty, //Condicional - Archivo en cadena Base64 del archivo XML que se va a timbrar. 
                    Id = $"{vSerie}{vFolio}", // Id unico para identificar el CFDI
                    ModoDebug = ModoDebug, // Indica si se generara errores e informacion detallada del proceso de timbrado del CFDI
                    ModoPrueba = ModoPrueba, // Indica si el CFDI se va a timbrar en el ambiente de pruebas
                    Password = ConfigurationManager.AppSettings["WSPassword"], //Contraseña del usuario para realizar la conexion con el servicio de timbrado
                    ProveedorPacDefault = ConfigurationManager.AppSettings["ProveedorPacDefault"], //Proveedor PAC con el que se realizara el timbrado
                    RfcEmisor = vRfcEmisor, // Registro Federal de Contribuyentes (RFC) del Emisor del CFDI
                    RutaArchivo = @"C:\Desarrollo\AppsMexico\GitHub\AppsMexico\AppsMexico.Integraciones\test\FE\prueba_cfdi33_ingreso.xml", // Condicional - Ruta fisica del archivo XML que se va a timbrar (Path). En caso de pasar el archivo en Base64 o arreglo de bytes, esta ruta sirve para indicar donde se van a grabar los archivos que genere el proceso.
                    Usuario = ConfigurationManager.AppSettings["WSUsuario"], //Usuario para realizar la conexion con el servicio de timbrado
                };

                SatProveedoresPacController SatProveedoresPacServiceBase = new SatProveedoresPacController();
                TimbradoCfdiResponse vTimbradoCfdiResponse = await SatProveedoresPacServiceBase.TimbrarCfdi(vTimbradoCfdiRequest);

                //TimbradoCfdiResponse vTimbradoCfdiResponse = new TimbradoCfdiResponse()
                //{
                //    Id //ID o Codigo de Mensaje de la respuesta
                //    Descripcion // Mensaje de la respuesta
                //    OperacionExitosa // Indica si el proceso se ejecuto exitosamente o no. True para respuestas exitosas y Falso para respuestas erroneas
                //    ArchivoXmlBase64 // Archivo en cadena Base64 del XML Encoding UTF8 en respustas exitosas
                //    RfcPac // RFC del proveedor PAC con el que se timbro el CFDI
                //};


                if (vTimbradoCfdiResponse.OperacionExitosa == false)
                {
                    if ((vTimbradoCfdiRequest.ModoPrueba == false && vTimbradoCfdiRequest.ModoDebug == false))
                    {
                        throw new ApplicationException($"Error en el proceso de timbrado del comprobante {vTimbradoCfdiRequest.Id}. Codigo de Error: {vTimbradoCfdiResponse.Id} - Descripcion: {vTimbradoCfdiResponse.Descripcion}");
                    }
                    else
                    {
                        throw new ApplicationException($"Error en el proceso de timbrado del comprobante {vTimbradoCfdiRequest.Id}. Codigo de Error: {vTimbradoCfdiResponse.Id} - Descripcion: {vTimbradoCfdiResponse.Descripcion} | Ruta con los archivos para su revision: {vTimbradoCfdiRequest.RutaArchivo}");
                    }
                }
                else if (string.IsNullOrWhiteSpace(vTimbradoCfdiResponse.ArchivoXmlBase64))
                {
                    throw new ApplicationException($"El comprobante {vTimbradoCfdiRequest.Id} se timbro correctamente, pero no se pudo obtener el XML. Favor de revisar en el SAT si existe el CFDI.");
                }

                string vXmlTimbrado = vTimbradoCfdiResponse.ArchivoXmlBase64.FromBase64String();
                System.IO.File.WriteAllBytes(vTimbradoCfdiRequest.RutaArchivo.Replace(".xml", ".timbrado.xml"), Convert.FromBase64String(vTimbradoCfdiResponse.ArchivoXmlBase64));
                Console.WriteLine($"PAC: {vTimbradoCfdiResponse.RfcPac}");
                Console.WriteLine($"XML Timbrado: {vXmlTimbrado}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ExceptionFunctions.GetExceptionMessage(ex, ModoDebug)}");
            }
        }
        #endregion

        #region Cancelar CFDI
        private static async Task CancelarCfdi()
        {
            try
            {
                string vMotivoCancelacionCodigo = "02";
                string vRfcEmisor = "XIA190128J61";
                string vRfcReceptor = "XAXX010101000";
                string vSelloDigital = "NaqoX3HicEWZKo8qlpWw+JoypFKqTqBYJzNaxrHM2M/QE02nK8YKOl0wX49lUyNjx1VQWOmoXMPrmCSbX5co0EkRspjZBQL+ee6fVB7D6K3rmthm/ZBnj451vqGx+80+OLBSJJvSawWoU9YJfqsRXeNaKimJk1L1f8bh27wDDSrsCrHQ0C9l+81APKEKAa250BxLljvc3XvxPETqxuNJYuJ80aGs2Vcz7+3BSB6h8HTQCbYd5W4fGu8xRYeTKMW89hpfRtQejsUC4z9fbT/juD+1Jpctd5h4Aw5VZJHtYYK04ijnlto7jpdRJ9O7X2vRx2XFOGDs2zYucXt6eb6cOQ==";
                double vTotal = 116;
                Guid vUUID = new Guid("BCDAFD18-C4E2-4558-A05D-6C41C710BF07");
                Guid? vUUIDSustitucion = null;

                CancelarCfdiRequest vCancelarCfdiRequest = new CancelarCfdiRequest()
                {
                    //ArchivoCSDCer = ArchivoCer, // Arreglo de bytes del certificado (.cer) del CSD
                    //ArchivoCSDCerBase64 = string.Empty,  // Archivo en cadena Base64 del certificado (.cer) del CSD
                    //ArchivoCSDEnc = ArchivoEnc, // Arreglo de bytes de la llave privada del certificado (.key) convertido a (.enc) del CSD
                    //ArchivoCSDEncBase64 = string.Empty,  // Archivo en cadena Base64 de la llave privada del certificado (.key) convertido a (.enc) del CSD
                    //ArchivoCSDPem = ArchivoPem, // Arreglo de bytes del certificado (.cer) convertido a (.pem) del CSD
                    //ArchivoCSDPemBase64 = string.Empty, // Archivo en cadena Base64 del certificado (.cer) convertido a (.pem) del CSD
                    //ArchivoCSDPfx = ArchivoPkcs12, // Arreglo de bytes del certificado (.cer) y llave privada (.key) del CSD convertido a (.pfx)
                    //ArchivoCSDPfxBase64 = string.Empty, // Archivo en cadena Base64 del certificado (.cer) y llave privada (.key) del CSD convertido a (.pfx)
                    //ArchivoCSDKey = ArchivoKey, // Arreglo de bytes de la llave privada del certificado (.key) del CSD
                    //ArchivoCSDKeyBase64 = string.Empty, // Archivo en cadena Base64 de la llave privada del certificado (.key) del CSD
                    ModoDebug = ModoDebug, // Indica si se generara errores e informacion detallada del proceso de timbrado del CFDI
                    ModoPrueba = ModoPrueba, // Indica si el CFDI se va a cancelar el CFDI en el ambiente de pruebas
                    MotivoCancelacionCodigo = vMotivoCancelacionCodigo, // Codigo con el motivo de la cancelacion del CFDI (01, 02, 03, 04)
                    Password = ConfigurationManager.AppSettings["WSPassword"], //Contraseña del usuario para realizar la conexion con el servicio de cancelaciones
                    PasswordArchivoCSDKey = ConfigurationManager.AppSettings["PasswordArchivoCSDKey"], // Contraseña de la llave privada del certificado (.key) del CSD
                    ProveedorPacDefault = ConfigurationManager.AppSettings["ProveedorPacDefault"], // Proveedor PAC con el que se realizo el timbrado del CFDI a cancelar
                    RfcEmisor = vRfcEmisor, // Registro Federal de Contribuyentes (RFC) del Emisor del CFDI
                    RfcReceptor = vRfcReceptor, // Registro Federal de Contribuyentes (RFC) del Receptor del CFDI
                    RutaArchivoAcuse = @"C:\Desarrollo\AppsMexico\GitHub\AppsMexico\AppsMexico.Integraciones\test\FE\prueba_cfdi33_ingreso_acuseCancelacion.xml", // Ruta fisica del archivo (Path) para grabar el acuse de cancelacion
                    RutaArchivoCSDCer = @"C:\Desarrollo\AppsMexico\GitHub\AppsMexico\AppsMexico.Integraciones\test\FE\CSD_XIA190128J61_20190617140806\CSD_Xenon_Industrial_Articles_XIA190128J61_20190617_140751s.cer", // Ruta fisica (Path) del certificado (.cer) del CSD
                    RutaArchivoCSDEnc = @"C:\Desarrollo\AppsMexico\GitHub\AppsMexico\AppsMexico.Integraciones\test\FE\CSD_XIA190128J61_20190617140806\XIA190128J61_ENC.enc", // Ruta fisica (Path) del certificado (.key) convertido a (.enc) del CSD
                    RutaArchivoCSDPem = @"C:\Desarrollo\AppsMexico\GitHub\AppsMexico\AppsMexico.Integraciones\test\FE\CSD_XIA190128J61_20190617140806\XIA190128J61_PEM.pem", // Ruta fisica (Path) del certificado (.cer) convertido a (.pem) del CSD
                    RutaArchivoCSDPfx = @"C:\Desarrollo\AppsMexico\GitHub\AppsMexico\AppsMexico.Integraciones\test\FE\CSD_XIA190128J61_20190617140806\XIA190128J61_PFX.pfx", // Ruta fisica (Path) del certificado (.cer) y llave privada (.key) del CSD convertido a (.pfx)
                    RutaArchivoCSDKey = @"C:\Desarrollo\AppsMexico\GitHub\AppsMexico\AppsMexico.Integraciones\test\FE\CSD_XIA190128J61_20190617140806\CSD_Xenon_Industrial_Articles_XIA190128J61_20190617_140751.key", // Ruta fisica (Path) del certificado (.key) del CSD
                    SelloDigital = vSelloDigital, // Sello Digital del CFDI a cancelar
                    Total = vTotal, // Total del CFDI a cancelar
                    Usuario = ConfigurationManager.AppSettings["WSUsuario"], //Usuario para realizar la conexion con el servicio de cancelaciones
                    UUID = vUUID, // Folio Fiscal (UUID) del CFDI a cancelar
                    UUIDSustitucion = vUUIDSustitucion // Folio Fiscal (UUID) del CFDI que esta sustituyendo al CFDI que se esta cancelando cuando el motivo de cancelacion sea 01
                };
                SatProveedoresPacController SatProveedoresPacServiceBase = new SatProveedoresPacController();
                CancelarCfdiResponse vCancelarCfdiResponse = await SatProveedoresPacServiceBase.CancelarCfdi(vCancelarCfdiRequest);

                //CancelarCfdiResponse vCancelarCfdiResponse = new CancelarCfdiResponse()
                //{
                //        Id //ID o Codigo de Mensaje de la respuesta
                //        Descripcion // Mensaje de la respuesta
                //        OperacionExitosa // Indica si el proceso se ejecuto exitosamente o no. True para respuestas exitosas y Falso para respuestas erroneas
                //        ArchivoXmlAcuseBase64 // Archivo en cadena Base64 del XML del acuse de cancelacion Encoding UTF8
                //        CodigoStatusSat // Estatus de la consulta del CFDI en el SAT
                //        DocumentosRelacionados // Listado de folios fiscales relacionados al CFDI que se quiere cancelar
                //        FechaSolicitudCancelacion // Fecha en la que se realizo la solicitud de cancelacion
                //        RfcPac // RFC del proveedor PAC con el que se cancelo el CFDI
                //        StatusCancelacion // Estatus de la solicitud de cancelacion
                //        StatusEsCancelable // Estatus para determinar si el CFDI es o no cancelable
                //};

                if (vCancelarCfdiResponse.DocumentosRelacionados != null && vCancelarCfdiResponse.DocumentosRelacionados.Count > 0)
                {
                    Console.WriteLine($"DocumentosRelacionados: {string.Join("|", vCancelarCfdiResponse.DocumentosRelacionados.Select(c => c.Uuid))}");
                }
                //FechaUltimaConsulta = DateTime.Now;
                //Observaciones = $"{vCancelarCfdiResponse.Id} - {vCancelarCfdiResponse.Descripcion}";
                //StatusCancelacion = vCancelarCfdiResponse.StatusCancelacion;
                //StatusCancelacionCodigo = vCancelarCfdiResponse.Id;
                //StatusCfdiSatCodigo = vCancelarCfdiResponse.CodigoStatusSat;
                //StatusEsCancelable = vCancelarCfdiResponse.StatusEsCancelable;


                if (vCancelarCfdiResponse.OperacionExitosa == true)
                {
                    if (string.IsNullOrWhiteSpace(vCancelarCfdiResponse.ArchivoXmlAcuseBase64))
                    {
                        throw new ApplicationException($"El comprobante {vUUID} se cancelo correctamente, pero no se pudo obtener el XML. Favor de revisar en el SAT el estatus del CFDI.");
                    }
                    else
                    {
                        string vXmlAcuse = vCancelarCfdiResponse.ArchivoXmlAcuseBase64.FromBase64String();
                        System.IO.File.WriteAllBytes(vCancelarCfdiRequest.RutaArchivoAcuse.Replace(".xml", ".acuse.xml"), Convert.FromBase64String(vCancelarCfdiResponse.ArchivoXmlAcuseBase64));
                        Console.WriteLine($"XML Acuse: {vXmlAcuse}");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {vCancelarCfdiResponse.Descripcion}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ExceptionFunctions.GetExceptionMessage(ex, ModoDebug)}");
            }
        }
        #endregion
    }
}
