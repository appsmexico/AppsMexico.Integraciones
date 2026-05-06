namespace FacturacionElectronica.Net.Xml.Model.Comprobante
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Clase para enviar la solicitud de timbrado de un CFDI
    /// </summary>
    public class TimbradoCfdiRequest
    {
        /// <summary>
        /// Id unico en el ERP o sistema de origen del documento que se desea timbrar, para su identificación en la base de datos y evitar timbrados duplicados del mismo documento
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Archivo XML del CFDI en formato Base64 para su timbrado. Nota: El archivo XML debe cumplir con el formato estandar de CFDI definido por el SAT para su correcta validación y timbrado
        /// </summary>
        public required string ArchivoBase64 { get; set; }

        /// <summary>
        /// Nombre de la aplicacion que esta ejecutando el proceso.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [MaxLength(100)]
        public string? AplicacionId { get; set; }

        /// <summary>
        /// Indica si posterior al timbrado exitoso del CFDI se debe generar la representacion impresa del CFDI (PDF)
        /// </summary>
        public bool GenerarPdf { get; set; }

        /// <summary>
        /// Indica si posterior al timbrado exitoso del CFDI se debe generar el codigo QR
        /// </summary>
        public bool GenerarQrCode { get; set; }

        /// <summary>
        /// Indica si se generara errores e informacion detallada del proceso de timbrado del CFDI
        /// </summary>
        public bool ModoDebug { get; set; }

        /// <summary>
        /// Indica si el CFDI se va a timbrar en el ambiente de pruebas
        /// </summary>        
        public bool ModoPrueba { get; set; }

        /// <summary>
        /// Codigo o Identificador del proceso desde el cual se esta generando el documento. Puede ser el numero de pantalla o proceso en el ERP para su identificación en la base de datos y generación de reportes de uso de la API por proceso o pantalla del ERP
        /// </summary>
        [MaxLength(100)]
        public required string ProcesoId { get; set; }


        /// <summary>
        /// Registro Federal de Contribuyentes (RFC) del Emisor del CFDI
        /// </summary>
        [RegularExpression(@"[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A-Z]?")]
        [StringLength(13, MinimumLength = 12)]
        public required string RfcEmisor { get; set; }


    }
}
