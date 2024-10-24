﻿//Se deben instalar estos paquetes NuGet para que el proceso se ejecute correctamente
//Agregar referncia a Nuget AppsMexico.Common.Sat.Pac_Integraciones desde URL https://cloud.appsmexico.mx/am/NugetServerPublic/nuget 
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
//<package id="System.Text.Json" version="5.0.0" targetFramework="net462" />
namespace FacturacionElectronica.NetFramework
{
    using AppsMexico.Common.Classes;
    using AppsMexico.Common.Classes.Account;
    using AppsMexico.Common.Classes.Dbo;
    using AppsMexico.Common.Classes.Enums;
    using AppsMexico.Common.Classes.Enums.Sat.Cfdi;
    using AppsMexico.Common.Classes.Sat;
    using AppsMexico.Common.Functions;
    using AppsMexico.Common.Sat.Pac;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    internal class Program
    {
        private static string ApiUrl { get => ConfigurationManager.AppSettings["ApiUrl"]; }
        private static bool ModoDebug { get => Convert.ToBoolean(ConfigurationManager.AppSettings["ModoDebug"]); }
        private static bool ModoPrueba { get => Convert.ToBoolean(ConfigurationManager.AppSettings["ModoPrueba"]); }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Ingrese el proceso que desea realizar. Timbrado XML SAT PAC Library = 1, Cancelacion XML SAT PAC Library = 2, Timbrado Ingreso JSON = 3, Timbrado Ingreso Carta Porte JSON = 4, Timbrado Traslado Carta Porte JSON = 5, Cancelacion CFDI JSON = 6, Timbrado XML AM ERP API = 7, Validar Estatus de un CFDI = 8");
                string vRespuesta = Console.ReadLine();
                if (string.Compare(vRespuesta, "1", true) == 0)
                {
                    Console.WriteLine($"{nameof(TimbrarCfdi)}");
                    Task.Run(async () => await TimbrarCfdi());
                }
                else if (string.Compare(vRespuesta, "2", true) == 0)
                {
                    Console.WriteLine($"{nameof(CancelarCfdi)}");
                    Task.Run(async () => await CancelarCfdi());
                }
                else if (string.Compare(vRespuesta, "3", true) == 0)
                {
                    Console.WriteLine($"{nameof(TimbrarCfdiIngresoJson)}");
                    Task.Run(async () => await TimbrarCfdiIngresoJson());
                }
                else if (string.Compare(vRespuesta, "4", true) == 0)
                {
                    Console.WriteLine($"{nameof(TimbrarCfdiIngresoCartaPorteJson)}");
                    Task.Run(async () => await TimbrarCfdiIngresoCartaPorteJson());
                }
                else if (string.Compare(vRespuesta, "5", true) == 0)
                {
                    Console.WriteLine($"{nameof(TimbrarCfdiTrasladoCartaPorteJson)}");
                    Task.Run(async () => await TimbrarCfdiTrasladoCartaPorteJson());
                }
                else if (string.Compare(vRespuesta, "6", true) == 0)
                {
                    Console.WriteLine($"{nameof(CancelarCfdiJson)}");
                    Task.Run(async () => await CancelarCfdiJson());
                }
                else if (string.Compare(vRespuesta, "7", true) == 0)
                {
                    Console.WriteLine($"{nameof(TimbrarCfdiByAMErpApi)}");
                    Task.Run(async () => await TimbrarCfdiByAMErpApi());
                }
                else if (string.Compare(vRespuesta, "8", true) == 0)
                {
                    Console.WriteLine($"{nameof(ValidarStatusCfdi)}");
                    Task.Run(async () => await ValidarStatusCfdi());
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ExceptionFunctions.GetExceptionMessage(ex, ModoDebug)}");
            }
            Console.WriteLine("Press ENTER to finish");
            Console.ReadLine();
        }


        #region Timbrar CFDI XML SAT PAC Library
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

        #region Cancelar CFDI XML SAT PAC Library
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


        #region Validar Status CFDI XML SAT PAC Library
        private static async Task ValidarStatusCfdi()
        {
            try
            {
                string vRfcEmisor = "MRE9603207K8";

                CancelarCfdiRequest vValidarStatusCfdiRequest = new CancelarCfdiRequest()
                {
                    ModoDebug = ModoDebug,
                    ModoPrueba = ModoPrueba,
                    Password = ConfigurationManager.AppSettings["WSPassword"], //Contraseña del usuario para realizar la conexion con el servicio de validacion
                    ProcesoId = "SAT5900", //Id del proceso que esta ejecutando el la validacion
                    ProveedorPacDefault = ConfigurationManager.AppSettings["ProveedorPacDefault"], // Proveedor PAC con el que se realizo el timbrado del CFDI a validar
                    RfcEmpresaTransaccion = vRfcEmisor, //RFC de la empresa que esta realizando la transaccion
                    RfcEmisor = vRfcEmisor, //RFC del emisor del CFDI
                    RfcReceptor = "XAXX010101000", //RFC del receptor del CFDI
                    SelloDigital = "C3B/wn+qiw0nS6B2sTQFykN7sy3At34GAE9XsKSc0HkCEV5Op+a9S32Q7EGEXT2Nwr3wSZU8Iq7WK8+yJQ2ArD8Gdyq4O25iQZtrHeJnd7fZHnIMSSJrNG7/Tspyta/jKytIZpLMwBpHlBDS5rJEMib6wn9z0zkj5OcHGHAi12SCN43rLa+yAp6m58OIdnYcpwhQryiSetTEZlzy6yOePDHmtGPV6q2B08ovEdWNV8Y2lGzyFgd9g7M0Et8KDhLmg/XLUTCAr5z0fk8DZElY3W+gGAiSTGYpRfDZwgEpDxBsLgsBpY2Y9EclEE993y9tLlsbXgl0WFZ0myKILOKcXw==", //Sello digital del CFDI
                    Total = 160.01, //Cuando el tipo de comprobante sea Pago, el Total debe ser 0
                    Usuario = ConfigurationManager.AppSettings["WSUsuario"], //Usuario para realizar la conexion con el servicio de validaciones
                    UUID = DataFunctions.GetGuid("79447A87-F6EB-4624-8377-7D93573BFD9C") //Folio Fiscal del CFDI
                };

                SatProveedoresPacController SatProveedoresPacServiceBase = new SatProveedoresPacController();
                CancelarCfdiResponse vValidarStatusCfdiResponse = await SatProveedoresPacServiceBase.ValidarStatusCfdi(vValidarStatusCfdiRequest);
                //vValidarStatusCfdiResponse.CodigoStatusSat == EnumCodigoStatusSat;
                //vValidarStatusCfdiResponse.StatusCancelacion == EnumStatusCancelacion;
                //vValidarStatusCfdiResponse.StatusEsCancelable == EnumStatusEsCancelable;

                Console.WriteLine($"Response: {vValidarStatusCfdiResponse.ToJson()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ExceptionFunctions.GetExceptionMessage(ex, ModoDebug)}");
            }
        }
        #endregion


        #region Timbrar CFDI XML AM ERP API
        private static async Task TimbrarCfdiByAMErpApi()
        {
            try
            {
                string vSerie = "A";
                string vFolio = "37";
                string vRfcEmisor = "XIA190128J61";

                TokenLogin vTokenLogin = await Login();
                if (!vTokenLogin.OperacionExitosa)
                {
                    throw new ApplicationException(vTokenLogin.Mensaje);
                }

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

                    GenerarPdf = true, //Indica si genera o no una representacion impresa del CFDI
                    GenerarQrCode = true, //Indica si genera o no el codigo QR de la representacion impresa del CFDI
                };

                TimbradoCfdiResponse vTimbradoCfdiResponse = null;
                string vData = WebFunctions.JsonConvert_SerializeObject(vTimbradoCfdiRequest);
                string vWebRequest = await WebFunctions.PostRequestBearerTokenAsync("/ErpComprobantes/TimbrarComprobanteErpFromXml", vData, token: vTokenLogin.Token, rootUrl: ApiUrl);
                try
                {
                    vTimbradoCfdiResponse = WebFunctions.JsonConvert_DeserializeObject<TimbradoCfdiResponse>(vWebRequest);
                }
                catch (Exception)
                {
                    throw new ApplicationException(WebFunctions.ConvertHtmlToString(vWebRequest));
                }

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

        #region Genera Token
        private static async Task<TokenLogin> Login()
        {
            try
            {
                TokenLogin vTokenLogin = null;

                Login vLogin = new Login()
                {
                    UserId = "democfdi@appsmexico.mx",
                    Password = "Pa$$w0rd",
                    RememberMe = true,
                    EmpresaId = "DEMO",
                    AplicacionId = AssemblyInfo.Title,
                    BrowserSOVersion = "",
                    DispositivoId = Environment.MachineName,
                    ClientIPAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString()
                };

                string vData = WebFunctions.JsonConvert_SerializeObject(vLogin);
                string vWebRequest = await WebFunctions.PostRequestAsync($"{ApiUrl}/Account/Login", vData);
                try
                {
                    vTokenLogin = WebFunctions.JsonConvert_DeserializeObject<TokenLogin>(vWebRequest);
                }
                catch (Exception)
                {
                    throw new ApplicationException(WebFunctions.ConvertHtmlToString(vWebRequest));
                }

                if (vTokenLogin == null)
                {
                    throw new ApplicationException("No se obtuvo informacion del token para iniciar sersion.");
                }
                else if (vTokenLogin.OperacionExitosa == false)
                {
                    throw new ApplicationException(vTokenLogin.Mensaje);
                }

                return vTokenLogin;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Cancelar CFDI JSON
        private static async Task CancelarCfdiJson()
        {
            try
            {
                TokenLogin vTokenLogin = await Login();

                ComprobanteCancelacionDTO vComprobanteCancelacionDTO = new ComprobanteCancelacionDTO()
                {
                    EmpresaId = "DEMO",
                    FechaCancelado = DateTime.Now,
                    MotivoCancelacion = "Se cancela por ser un documento de prueba",
                    MotivoCancelacionCodigo = "02",
                    UUID = new Guid("3B5DE2A3-5C90-5F17-AC16-F675FE74AEBE")
                };

                CancelarCfdiResponse vCancelarCfdiResponse = new CancelarCfdiResponse(false);
                try
                {
                    string vData = WebFunctions.JsonConvert_SerializeObject(vComprobanteCancelacionDTO);
                    string vWebRequest = await WebFunctions.PostRequestBearerTokenAsync($"{ApiUrl}/ComprobantesCancelacion/CancelarComprobanteByUUID", vData, vTokenLogin.Token);
                    try
                    {
                        vCancelarCfdiResponse = WebFunctions.JsonConvert_DeserializeObject<CancelarCfdiResponse>(vWebRequest);
                    }
                    catch (Exception)
                    {
                        throw new ApplicationException(WebFunctions.ConvertHtmlToString(vWebRequest));
                    }
                }
                catch (Exception ex)
                {
                    vCancelarCfdiResponse.Descripcion = ExceptionFunctions.GetExceptionMessage(ex, ModoDebug);
                }

                if (vCancelarCfdiResponse.OperacionExitosa)
                {
                    string vDownloadsFolder = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
                    if (System.IO.Directory.Exists(vDownloadsFolder))
                    {
                        System.IO.File.WriteAllBytes(System.IO.Path.Combine(vDownloadsFolder, $"{vCancelarCfdiResponse.Id}.acuse.xml"), Convert.FromBase64String(vCancelarCfdiResponse.ArchivoXmlAcuseBase64));
                    }
                }

                Console.WriteLine($"{vCancelarCfdiResponse.ToJson()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ExceptionFunctions.GetExceptionMessage(ex, ModoDebug)}");
            }
        }
        #endregion

        #region Timbrar CFDI INGRESO JSON
        private static async Task TimbrarCfdiIngresoJson()
        {
            try
            {
                TokenLogin vTokenLogin = await Login();

                ComprobanteDTO vComprobanteDTO = new ComprobanteDTO()
                {
                    EmpresaId = "DEMO",
                    ErpId = "PRCP000003",
                    ReferenciaErp = "TMPIN16215",
                    ClienteId = "0000000118",
                    Comentarios = "",
                    Complemento = "",
                    CondicionesDePago = "CONTADO",
                    Descuento = 0.0,
                    DireccionEnvioId = "DEFAULT",
                    EmisorFacAtrAdquiriente = "",
                    EmisorRazonSocial = "",
                    EmisorRegimenFiscalId = "",
                    EmisorRfc = "",
                    ErpTipoDocumento = "IN",
                    Exportacion = "",
                    Fecha = DateTime.Now,
                    Folio = "",
                    FormaDePago = "03",
                    InformacionGlobalAnio = null,
                    InformacionGlobalMeses = null,
                    InformacionGlobalPeriodicidad = null,
                    LugarExpedicion = "61957",
                    MetodoDePago = "PUE",
                    ModuloErp = "VENTAS",
                    MonedaId = "MXN",
                    MotivoDescuento = "",
                    NombreReporte = "",
                    OrdenCompra = "",
                    PacRfc = "",
                    PedidoId = "",
                    ReceptorEmail = "",
                    ReceptorRazonSocial = "VENTA AL PUBLICO EN GENERAL",
                    ReceptorRegimenFiscalId = "616",
                    ReceptorRfc = "XAXX010101000",
                    ReceptorCalle = "AV. DE LOS MAESTROS ",
                    ReceptorCiudadId = "",
                    ReceptorCodigoPostalId = "61957",
                    ReceptorColonia = "ALCALDE BARRANQUITAS",
                    ReceptorCurp = "",
                    ReceptorEnviarEmailAutomaticamente = true,
                    ReceptorEstadoId = "CHI",
                    ReceptorMostrarDomicilioFiscal = true,
                    ReceptorMunicipioId = "HIDALGO DEL PARRAL",
                    ReceptorNombreComercial = "",
                    ReceptorNumeroExterior = "284",
                    ReceptorNumeroInterior = "2",
                    ReceptorPaisId = "MX",
                    ReceptorRegistroIdentificacionFiscal = "",
                    ReceptorUsoCfdi = "S01",
                    Saldo = 232.0,
                    Serie = "FA",
                    SerieAtributo = "",
                    SubTotal = 200.0,
                    SucursalAtributo = "",
                    SucursalId = "DEMO",
                    TimbrarComprobanteAutomaticamente = true,
                    TipoCambio = 1.0,
                    TipoDocumentoId = "IN",
                    TipoRelacionComprobantes = "",
                    Total = 232.0,
                    TotalImpuestosLocalesRetenidos = 0.0,
                    TotalImpuestosLocalesTrasladados = 0.0,
                    TotalImpuestosRetenidos = 0.0,
                    TotalImpuestosTrasladados = 32.0,
                    ValidarInformacionWebApp = false,
                    Conceptos = new List<ComprobanteConceptoDTO>(),
                };

                ComprobanteConceptoDTO vComprobanteConceptoDTO = new ComprobanteConceptoDTO()
                {
                    ConceptoId = "1",
                    Cantidad = 2.0,
                    ComentariosConcepto = "",
                    Complemento = "",
                    Descripcion = "PRUEBA CFDI 4.0",
                    DescuentoConcepto = 0.0,
                    ErpIdConcepto = "1",
                    GrupoImpuestos = "",
                    Importe = 200.0,
                    NoIdentificacion = "4201000",
                    ObjetoDeImpuestos = "02",
                    PrecioBase = 100.0,
                    ProductoServicioId = "",
                    UnidadMedida = "SERV",
                    UnidadMedidaClave = "",
                    ValorUnitario = 100.0,
                    Impuestos = new List<ComprobanteImpuestoDTO>() {
                        new ComprobanteImpuestoDTO
                        {
                            LineaImpuesto = 1,
                            CodigoImpuesto = "IVA16",
                            ImporteBase = 200.0,
                            ImporteImpuesto = 32.0,
                            TasaOCuota = 0.16,
                            TipoFactor = c_TipoFactor.Tasa,
                            TipoImpuesto = EnumTipoImpuesto.T
                        }
                    },
                };

                vComprobanteDTO.Conceptos.Add(vComprobanteConceptoDTO);

                ApiResponse vApiResponse = new ApiResponse(false);
                try
                {
                    string vData = WebFunctions.JsonConvert_SerializeObject(vComprobanteDTO);
                    string vWebRequest = await WebFunctions.PostRequestBearerTokenAsync($"{ApiUrl}/ComprobantesErp/Comprobante", vData, vTokenLogin.Token);
                    try
                    {
                        vApiResponse = WebFunctions.JsonConvert_DeserializeObject<ApiResponse>(vWebRequest);
                    }
                    catch (Exception)
                    {
                        throw new ApplicationException(WebFunctions.ConvertHtmlToString(vWebRequest));
                    }
                }
                catch (Exception ex)
                {
                    vApiResponse.Descripcion = ExceptionFunctions.GetExceptionMessage(ex, ModoDebug);
                }

                if (vApiResponse.OperacionExitosa)
                {
                    Console.WriteLine($"Folio Fiscal (UUID): {vApiResponse.Id}");
                    await GetComprobanteCfdiDTOById(vApiResponse.Id, vTokenLogin.Token);
                }

                Console.WriteLine($"{vApiResponse.ToJson()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ExceptionFunctions.GetExceptionMessage(ex, ModoDebug)}");
            }
        }
        #endregion

        #region Timbrar CFDI INGRESO CARTA PORTE JSON
        private static async Task TimbrarCfdiIngresoCartaPorteJson()
        {
            try
            {
                TokenLogin vTokenLogin = await Login();

                ComprobanteDTO vComprobanteDTO = new ComprobanteDTO
                {
                    EmpresaId = "DEMO",
                    ErpId = "PRCP000004",
                    ReferenciaErp = "TMPIN16215",
                    ClienteId = "0000000001",
                    ClienteIdFactoraje = "",
                    AnioFiscal = 2024,
                    Comentarios = "",
                    Complemento = "",
                    CondicionesDePago = "CONTADO",
                    Descuento = 1.0,
                    DireccionEnvioId = "DEFAULT",
                    EmisorFacAtrAdquiriente = "",
                    EmisorRazonSocial = "",
                    EmisorRegimenFiscalId = "",
                    EmisorRfc = "",
                    ErpTipoDocumento = "IN",
                    Exportacion = "01",
                    Fecha = DateTime.Now,
                    Folio = "",
                    FormaDePago = "99",
                    InformacionGlobalAnio = null,
                    InformacionGlobalMeses = null,
                    InformacionGlobalPeriodicidad = null,
                    LugarExpedicion = "44270",
                    MetodoDePago = "PPD",
                    ModuloErp = "VENTAS",
                    MonedaId = "MXN",
                    MotivoDescuento = "",
                    NombreReporte = "",
                    OrdenCompra = "",
                    PacRfc = "",
                    PedidoId = "",
                    ReceptorEmail = "",
                    ReceptorRazonSocial = "INDISTRIA ILUMINADORA DE ALMACENES",
                    ReceptorRegimenCapital = "SA DE CV",
                    ReceptorRegimenFiscalId = "601",
                    ReceptorRfc = "IIA040805DZ4",
                    ReceptorCalle = "AV. PATRIA",
                    ReceptorCiudadId = "",
                    ReceptorCodigoPostalId = "44970",
                    ReceptorColonia = "ECHEVERRIA",
                    ReceptorCurp = "",
                    ReceptorEnviarEmailAutomaticamente = true,
                    ReceptorEstadoId = "JAL",
                    ReceptorMostrarDomicilioFiscal = true,
                    ReceptorMunicipioId = "GUADALAJARA",
                    ReceptorNombreComercial = "",
                    ReceptorNumeroExterior = "2",
                    ReceptorNumeroInterior = "B",
                    ReceptorPaisId = "MEX",
                    ReceptorRegistroIdentificacionFiscal = "",
                    ReceptorUsoCfdi = "G01",
                    Saldo = 198.95,
                    Serie = "A",
                    SerieAtributo = "",
                    SubTotal = 200.0,
                    SucursalAtributo = "",
                    SucursalId = "MATRIZ",
                    TimbrarComprobanteAutomaticamente = true,
                    TipoCambio = 1.0,
                    TipoDocumentoId = "IN",
                    TipoRelacionComprobantes = "",
                    Total = 198.95,
                    TotalImpuestosLocalesRetenidos = 0.0,
                    TotalImpuestosLocalesTrasladados = 0.0,
                    TotalImpuestosRetenidos = 0.21,
                    TotalImpuestosTrasladados = 0.16,
                    ValidarInformacionWebApp = false,
                    Conceptos = new List<ComprobanteConceptoDTO>(),
                };

                ComprobanteConceptoDTO vComprobanteConceptoDTO = new ComprobanteConceptoDTO
                {
                    ConceptoId = "1",
                    Cantidad = 1.0,
                    ComentariosConcepto = "",
                    Complemento = "",
                    Descripcion = "Brocolis",
                    DescuentoConcepto = 1.0,
                    ErpIdConcepto = "1",
                    GrupoImpuestos = "",
                    Importe = 200.0,
                    NoIdentificacion = "",
                    ObjetoDeImpuestos = "02",
                    PrecioBase = 200.0,
                    ProductoServicioId = "50442000",
                    UnidadMedida = "KILO",
                    UnidadMedidaClave = "KGM",
                    ValorUnitario = 200.0,
                    Impuestos = new List<ComprobanteImpuestoDTO> {
                        new ComprobanteImpuestoDTO
                        {
                            LineaImpuesto = 1,
                            CodigoImpuesto = "IVA16",
                            ImporteBase = 1.0,
                            ImporteImpuesto = 0.16,
                            TasaOCuota = 0.16,
                            TipoFactor = c_TipoFactor.Tasa,
                            TipoImpuesto = EnumTipoImpuesto.T
                        },
                        new ComprobanteImpuestoDTO
                        {
                            LineaImpuesto = 2,
                            CodigoImpuesto = "ISRRETH",
                            ImporteBase = 1.0,
                            ImporteImpuesto = 0.1,
                            TasaOCuota = 0.10,
                            TipoFactor = c_TipoFactor.Tasa,
                            TipoImpuesto = EnumTipoImpuesto.R
                        },
                        new ComprobanteImpuestoDTO
                        {
                            LineaImpuesto = 3,
                            CodigoImpuesto = "IVARETH",
                            ImporteBase = 1.0,
                            ImporteImpuesto = 0.11,
                            TasaOCuota = 0.106666,
                            TipoFactor = c_TipoFactor.Tasa,
                            TipoImpuesto = EnumTipoImpuesto.R
                        },
                    },
                    ComplementoCartaPorteMercancia = new ComprobanteConceptoComplementoCartaPorteMercanciaDTO
                    {
                        ConceptoId = "1",
                        PesoEnKg = 1.0,
                        MaterialPeligroso = false,
                        ClaveMaterialPeligroso = "",
                        ClaveEmbalaje = "",
                        ProductoServicioIdCP = "50442000",
                        CantidadesTransporta = new List<ComprobanteConceptoComplementoCartaPorteMercanciaCantidadTransportaDTO>
                        {
                            new ComprobanteConceptoComplementoCartaPorteMercanciaCantidadTransportaDTO
                            {
                                IDOrigen = "OR101010",
                                IDDestino = "DE202020",
                                Cantidad = 1,
                            },
                            new ComprobanteConceptoComplementoCartaPorteMercanciaCantidadTransportaDTO
                            {
                                IDOrigen = "OR101010",
                                IDDestino = "DE202021",
                                Cantidad = 1,
                            },
                        },
                    }
                };

                vComprobanteDTO.Conceptos.Add(vComprobanteConceptoDTO);

                vComprobanteDTO.ComplementoCartaPorte = new ComprobanteComplementoCartaPorteDTO
                {
                    TransporteInternacional = "No",
                    TotalDistanciaRecorrida = 2,
                    MercanciasPesoBrutoTotal = 2.0,
                    MercanciasUnidadPeso = "XBX",
                    MercanciasNumeroTotalMercancias = 2,
                    NumeroPermisoSCT = "NumPermisoSCT",
                    TipoPermisoSCT = "TPAF01",
                    Version = "3.1", //Cambio 3.1
                    IdCCP = DataFunctions.GetGuid($"CCC{Guid.NewGuid().ToString("N").Substring(3, 29)}"),
                    PesoBrutoVehicular = 10.5,
                    MercanciasLogisticaInversaRecoleccionDevolucion = true,
                    AutoTransporte = new ComprobanteComplementoCartaPorteAutoTransporteDTO
                    {
                        ErpId = "PLAC892", //Cambio 3.1 > Id unico del registro del vehiculo en el ERP
                        ConfiguracionVehicular = c_ConfigAutotransporte.VL, //Cambio 3.1 > Cambio de string por enumeracion
                        PlacaVM = "PLAC892",
                        AnioModeloVM = 2020,
                        SeguroAseguraResponsabilidadCivil = "SW Seguros",
                        SeguroPolizaResponsabilidadCivil = "123456789",
                        SeguroAseguraCarga = "SW Seguros",
                        SeguroAseguraMedioAmbiente = "SW Seguros Ambientales",
                        SeguroPolizaMedioAmbiente = "123456789",
                        Remolque1Placa = "ABC123",
                        Remolque1SubTipoRemolque = "CTR021",
                    },
                    FigurasTransporte = new List<ComprobanteComplementoCartaPorteFiguraTransporteDTO>
                    {
                        new ComprobanteComplementoCartaPorteFiguraTransporteDTO
                        {
                            ErpId = "CHOF123", //Cambio 3.1 > Id unico del registro de la figura de transporte en el ERP (PK Chofer)
                            Nombre = "Juan Perez", //Cambio 3.1 > Nombre de la figura de transporte (chofer)
                            TipoFiguraTransporte = EnumTipoFiguraTransporte.Operador,
                            Rfc = "VAAM130719H60",
                            NumeroLicencia = "a234567890",
                        }
                    },
                    Ubicaciones = new List<ComprobanteComplementoCartaPorteUbicacionDTO>
                    {
                        new ComprobanteComplementoCartaPorteUbicacionDTO
                        {
                            IDUbicacion = "OR101010",
                            TipoUbicacion = EnumTipoUbicacionTransporte.Origen,
                            RfcRemitenteDestinatario = "IVD920810GU2",
                            FechaHoraSalidaLlegada = new DateTime(2022,04,14),
                            Domicilio = new DomicilioDTO
                            {
                                CodigoDomicilio = "101010",
                                Calle = "calle",
                                NumeroExterior = "211",
                                Colonia = "0347",
                                Ciudad = "23",
                                Referencia = "casa blanca 1",
                                Municipio = "004",
                                Estado = "COA",
                                Pais = "MEX",
                                CodigoPostal = "25350"
                            },
                        },
                        new ComprobanteComplementoCartaPorteUbicacionDTO
                        {
                            IDUbicacion = "DE202020",
                            TipoUbicacion = EnumTipoUbicacionTransporte.Destino,
                            RfcRemitenteDestinatario = "AAA010101AAA",
                            FechaHoraSalidaLlegada = new DateTime(2022,04,14,1,0,0),
                            DistanciaRecorrida = 1,
                            Domicilio = new DomicilioDTO
                            {
                                CodigoDomicilio = "202020",
                                Calle = "calle",
                                NumeroExterior = "214",
                                Colonia = "0347",
                                Ciudad = "23",
                                Referencia = "casa blanca 2",
                                Municipio = "004",
                                Estado = "COA",
                                Pais = "MEX",
                                CodigoPostal = "25350"
                            },
                        },
                        new ComprobanteComplementoCartaPorteUbicacionDTO
                        {
                            IDUbicacion = "DE202021",
                            TipoUbicacion = EnumTipoUbicacionTransporte.Destino,
                            RfcRemitenteDestinatario = "AAA010101AAA",
                            FechaHoraSalidaLlegada = new DateTime(2022,04,14,2,0,0),
                            DistanciaRecorrida = 1,
                            Domicilio = new DomicilioDTO
                            {
                                CodigoDomicilio = "202021",
                                Calle = "calle",
                                NumeroExterior = "220",
                                Colonia = "0347",
                                Ciudad = "23",
                                Referencia = "casa blanca 3",
                                Municipio = "004",
                                Estado = "COA",
                                Pais = "MEX",
                                CodigoPostal = "25350"
                            },
                        },
                    }
                };
                ApiResponse vApiResponse = new ApiResponse(false);
                try
                {
                    string vData = WebFunctions.JsonConvert_SerializeObject(vComprobanteDTO);
                    string vWebRequest = await WebFunctions.PostRequestBearerTokenAsync($"{ApiUrl}/ComprobantesErp/Comprobante", vData, vTokenLogin.Token);
                    try
                    {
                        vApiResponse = WebFunctions.JsonConvert_DeserializeObject<ApiResponse>(vWebRequest);
                    }
                    catch (Exception)
                    {
                        throw new ApplicationException(WebFunctions.ConvertHtmlToString(vWebRequest));
                    }
                }
                catch (Exception ex)
                {
                    vApiResponse.Descripcion = ExceptionFunctions.GetExceptionMessage(ex, ModoDebug);
                }

                if (vApiResponse.OperacionExitosa)
                {
                    Console.WriteLine($"Folio Fiscal (UUID): {vApiResponse.Id}");
                    await GetComprobanteCfdiDTOById(vApiResponse.Id, vTokenLogin.Token);
                }

                Console.WriteLine($"{vApiResponse.ToJson()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ExceptionFunctions.GetExceptionMessage(ex, ModoDebug)}");
            }
        }
        #endregion

        #region Timbrar CFDI TRASLADO CARTA PORTE JSON
        private static async Task TimbrarCfdiTrasladoCartaPorteJson()
        {
            try
            {
                TokenLogin vTokenLogin = await Login();

                ComprobanteDTO vComprobanteDTO = new ComprobanteDTO
                {
                    EmpresaId = "DEMO",
                    ErpId = "CP0000142",
                    ReferenciaErp = "",
                    ClienteId = "0000000002",
                    ClienteIdFactoraje = "",
                    AnioFiscal = 2024,
                    Comentarios = "",
                    Complemento = "",
                    CondicionesDePago = "",
                    Descuento = 0.0,
                    DireccionEnvioId = "DEFAULT",
                    EmisorFacAtrAdquiriente = "",
                    EmisorRazonSocial = "",
                    EmisorRegimenFiscalId = "",
                    EmisorRfc = "",
                    ErpTipoDocumento = "T",
                    Exportacion = "01",
                    Fecha = DateTime.Now,
                    Folio = "",
                    FormaDePago = "",
                    InformacionGlobalAnio = null,
                    InformacionGlobalMeses = null,
                    InformacionGlobalPeriodicidad = null,
                    LugarExpedicion = "44270",
                    MetodoDePago = "",
                    ModuloErp = "ERP",
                    MonedaId = "XXX",
                    MotivoDescuento = "",
                    NombreReporte = "",
                    OrdenCompra = "",
                    PacRfc = "",
                    PedidoId = "",
                    ReceptorEmail = "",
                    ReceptorRazonSocial = "ESCUELA KEMPER URGATE",
                    ReceptorRegimenCapital = "SA DE CV",
                    ReceptorRegimenFiscalId = "601",
                    ReceptorRfc = "EKU9003173C9",
                    ReceptorCalle = "AV. PATRIA",
                    ReceptorCiudadId = "",
                    ReceptorCodigoPostalId = "44970",
                    ReceptorColonia = "ECHEVERRIA",
                    ReceptorCurp = "",
                    ReceptorEnviarEmailAutomaticamente = true,
                    ReceptorEstadoId = "JAL",
                    ReceptorMostrarDomicilioFiscal = true,
                    ReceptorMunicipioId = "GUADALAJARA",
                    ReceptorNombreComercial = "",
                    ReceptorNumeroExterior = "2",
                    ReceptorNumeroInterior = "B",
                    ReceptorPaisId = "MEX",
                    ReceptorRegistroIdentificacionFiscal = "",
                    ReceptorUsoCfdi = "P01",
                    Saldo = 0.0,
                    Serie = "TR",
                    SerieAtributo = "",
                    SubTotal = 0.0,
                    SucursalAtributo = "",
                    SucursalId = "MATRIZ",
                    TimbrarComprobanteAutomaticamente = true,
                    TipoCambio = 1.0,
                    TipoDocumentoId = "IN",
                    TipoRelacionComprobantes = "",
                    Total = 0.0,
                    TotalImpuestosLocalesRetenidos = 0.0,
                    TotalImpuestosLocalesTrasladados = 0.0,
                    TotalImpuestosRetenidos = 0.0,
                    TotalImpuestosTrasladados = 0.0,
                    ValidarInformacionWebApp = false,
                    Conceptos = new List<ComprobanteConceptoDTO>(),
                };

                ComprobanteConceptoDTO vComprobanteConceptoDTO = new ComprobanteConceptoDTO
                {
                    ConceptoId = "1",
                    Cantidad = 1.0,
                    ComentariosConcepto = "",
                    Complemento = "",
                    Descripcion = "PLATAFORMA ARTICULADA. HR2056",
                    DescuentoConcepto = 0.0,
                    ErpIdConcepto = "1",
                    GrupoImpuestos = "",
                    Importe = 0.0,
                    NoIdentificacion = "",
                    ObjetoDeImpuestos = "01",
                    PrecioBase = 0.0,
                    ProductoServicioId = "22101800",
                    UnidadMedida = "Pieza",
                    UnidadMedidaClave = "H87",
                    ValorUnitario = 0.0,
                    Impuestos = null,
                    ComplementoCartaPorteMercancia = new ComprobanteConceptoComplementoCartaPorteMercanciaDTO
                    {
                        ConceptoId = "1",
                        PesoEnKg = 7890.0,
                        MaterialPeligroso = false,
                        ClaveMaterialPeligroso = "",
                        ClaveEmbalaje = "",
                        CantidadesTransporta = new List<ComprobanteConceptoComplementoCartaPorteMercanciaCantidadTransportaDTO>
                        {
                            new ComprobanteConceptoComplementoCartaPorteMercanciaCantidadTransportaDTO
                            {
                                IDOrigen = "OR101010",
                                IDDestino = "DE202020",
                                Cantidad = 1,
                            },
                            new ComprobanteConceptoComplementoCartaPorteMercanciaCantidadTransportaDTO
                            {
                                IDOrigen = "OR101010",
                                IDDestino = "DE202021",
                                Cantidad = 1,
                            },
                        },
                    }
                };

                vComprobanteDTO.Conceptos.Add(vComprobanteConceptoDTO);

                vComprobanteDTO.ComplementoCartaPorte = new ComprobanteComplementoCartaPorteDTO
                {
                    RegistroISTMO = false,
                    TransporteInternacional = "No",
                    TotalDistanciaRecorrida = 188.07,
                    MercanciasCargoPorTasacion = 0.0,
                    MercanciasLogisticaInversaRecoleccionDevolucion = true,
                    MercanciasNumeroTotalMercancias = 1,
                    MercanciasPesoBrutoTotal = 7890.0,
                    MercanciasPesoNetoTotal = 0.0,
                    MercanciasUnidadPeso = "KGM",
                    NumeroPermisoSCT = "2857806",
                    PesoBrutoVehicular = 27.89,
                    TipoPermisoSCT = "TPAF01",
                    Version = "3.1", //Cambio 3.1
                    ViaEntradaSalida = "",
                    IdCCP = DataFunctions.GetGuid($"CCC{Guid.NewGuid().ToString("N").Substring(3, 29)}"),
                    AutoTransporte = new ComprobanteComplementoCartaPorteAutoTransporteDTO
                    {
                        ErpId = "69AZ1A", //Cambio 3.1 > Id unico del registro del vehiculo en el ERP
                        ConfiguracionVehicular = c_ConfigAutotransporte.C3, //Cambio 3.1 > Cambio de string por enumeracion
                        PlacaVM = "69AZ1A",
                        AnioModeloVM = 2023,
                        SeguroAseguraResponsabilidadCivil = "CHUBB",
                        SeguroPolizaResponsabilidadCivil = "GN 44012324",
                        SeguroAseguraCarga = "SW Seguros",
                        SeguroAseguraMedioAmbiente = "",
                        SeguroPolizaMedioAmbiente = "",
                        Remolque1Placa = "",
                        Remolque1SubTipoRemolque = "",
                    },
                    FigurasTransporte = new List<ComprobanteComplementoCartaPorteFiguraTransporteDTO>
                    {
                        new ComprobanteComplementoCartaPorteFiguraTransporteDTO
                        {
                            ErpId = "CHOF1234", //Cambio 3.1 > Id unico del registro de la figura de transporte en el ERP (PK Chofer)
                            Nombre = "OSCAR KALA HAAK", //Cambio 3.1 > Nombre de la figura de transporte (chofer)
                            TipoFiguraTransporte = EnumTipoFiguraTransporte.Operador,
                            Rfc = "KAHO641101B39",
                            NumeroLicencia = "LIC234567890",
                            ResidenciaFiscal = "",
                            NumeroRegistroIdentificacionFiscal = "",
                        }
                    },
                    Ubicaciones = new List<ComprobanteComplementoCartaPorteUbicacionDTO>
                    {
                        new ComprobanteComplementoCartaPorteUbicacionDTO
                        {
                            IDUbicacion = "OR101010",
                            TipoUbicacion = EnumTipoUbicacionTransporte.Origen,
                            NombreRemitenteDestinatario= "ESCUELA WILSON ESQUIVEL",
                            RfcRemitenteDestinatario = "EWE1709045U0",
                            FechaHoraSalidaLlegada = new DateTime(2024,07,24, 18,1,0),
                            Domicilio = new DomicilioDTO
                            {
                                CodigoDomicilio = "101010",
                                Calle = "calle",
                                NumeroExterior = "211",
                                Colonia = "0347",
                                Ciudad = "23",
                                Referencia = "casa blanca 1",
                                Municipio = "004",
                                Estado = "COA",
                                Pais = "MEX",
                                CodigoPostal = "25350"
                            },
                        },
                        new ComprobanteComplementoCartaPorteUbicacionDTO
                        {
                            IDUbicacion = "DE202020",
                            TipoUbicacion = EnumTipoUbicacionTransporte.Destino,
                            NombreRemitenteDestinatario= "KERNEL INDUSTIA JUGUETERA",
                            RfcRemitenteDestinatario = "KIJ0906199R1",
                            FechaHoraSalidaLlegada = new DateTime(2024,07,24, 19,0,0),
                            DistanciaRecorrida = 188.07,
                            Domicilio = new DomicilioDTO
                            {
                                CodigoDomicilio = "202020",
                                Calle = "calle",
                                NumeroExterior = "214",
                                Colonia = "0347",
                                Ciudad = "23",
                                Referencia = "casa blanca 2",
                                Municipio = "004",
                                Estado = "COA",
                                Pais = "MEX",
                                CodigoPostal = "25350"
                            },
                        }
                    }
                };
                ApiResponse vApiResponse = new ApiResponse(false);
                try
                {
                    string vData = WebFunctions.JsonConvert_SerializeObject(vComprobanteDTO);
                    string vWebRequest = await WebFunctions.PostRequestBearerTokenAsync($"{ApiUrl}/ComprobantesErp/Comprobante", vData, vTokenLogin.Token);
                    try
                    {
                        vApiResponse = WebFunctions.JsonConvert_DeserializeObject<ApiResponse>(vWebRequest);
                    }
                    catch (Exception)
                    {
                        throw new ApplicationException(WebFunctions.ConvertHtmlToString(vWebRequest));
                    }
                }
                catch (Exception ex)
                {
                    vApiResponse.Descripcion = ExceptionFunctions.GetExceptionMessage(ex, ModoDebug);
                }

                if (vApiResponse.OperacionExitosa)
                {
                    Console.WriteLine($"Folio Fiscal (UUID): {vApiResponse.Id}");
                    await GetComprobanteCfdiDTOById(vApiResponse.Id, vTokenLogin.Token);
                }

                Console.WriteLine($"{vApiResponse.ToJson()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ExceptionFunctions.GetExceptionMessage(ex, ModoDebug)}");
            }
        }
        #endregion

        #region Obtiene los datos del CFDI Timbrado
        private static async Task GetComprobanteCfdiDTOById(string id, string token)
        {
            try
            {
                string vDownloadsFolder = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
                if (System.IO.Directory.Exists(vDownloadsFolder))
                {
                    string vWebRequest = await WebFunctions.GetRequestBearerTokenAsync($"{ApiUrl}/Comprobantes/GetComprobanteCfdiDTOById?id={id}", token);
                    try
                    {
                        ComprobanteCfdiDTO vComprobanteCfdiDTO = WebFunctions.JsonConvert_DeserializeObject<ComprobanteCfdiDTO>(vWebRequest);
                        if (vComprobanteCfdiDTO != null)
                        {
                            if (vComprobanteCfdiDTO.ArchivoPdf != null && string.IsNullOrEmpty(vComprobanteCfdiDTO.ArchivoPdf.ArchivoBase64) == false)
                            {
                                System.IO.File.WriteAllBytes(System.IO.Path.Combine(vDownloadsFolder, vComprobanteCfdiDTO.ArchivoPdf.NombreArchivo), Convert.FromBase64String(vComprobanteCfdiDTO.ArchivoPdf.ArchivoBase64));
                            }

                            if (vComprobanteCfdiDTO.ArchivoXml != null && string.IsNullOrEmpty(vComprobanteCfdiDTO.ArchivoXml.ArchivoBase64) == false)
                            {
                                System.IO.File.WriteAllBytes(System.IO.Path.Combine(vDownloadsFolder, vComprobanteCfdiDTO.ArchivoXml.NombreArchivo), Convert.FromBase64String(vComprobanteCfdiDTO.ArchivoXml.ArchivoBase64));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw new ApplicationException(WebFunctions.ConvertHtmlToString(vWebRequest));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
