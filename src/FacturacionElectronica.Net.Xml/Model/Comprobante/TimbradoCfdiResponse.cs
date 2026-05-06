namespace FacturacionElectronica.Net.Xml.Model.Comprobante
{
    using FacturacionElectronica.Net.Xml.Model.Common;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase para enviar la respuesta de timbrado de un CFDI
    /// </summary>
    public class TimbradoCfdiResponse : ApiResponse
    {
        public TimbradoCfdiResponse() : base()
        {

        }

        public TimbradoCfdiResponse(bool operacionExitosa) : base(operacionExitosa)
        {

        }


        /// <summary>
        /// Archivo en cadena Base64 del PDF con la representacion impresa del CFDI
        /// </summary>
        public string? ArchivoPdfBase64 { get; set; }

        /// <summary>
        /// Archivo en cadena Base64 del PNG del codigo QR
        /// </summary>       
        public string? ArchivoQrCodeBase64 { get; set; }

        /// <summary>
        /// Archivo en cadena Base64 del XML Encoding UTF8
        /// </summary>
        public string? ArchivoXmlBase64 { get; set; }

        /// <summary>
        /// Archivo en cadena Base64 del XML de la solicitud de timbrado Encoding UTF8
        /// </summary>
        public string? ArchivoXmlRequestBase64 { get; set; }

        /// <summary>
        /// Archivo en cadena Base64 del XML de la respusta de timbrado Encoding UTF8
        /// </summary>       
        public string? ArchivoXmlResponseBase64 { get; set; }

        /// <summary>
        /// Cadena original del CFDI
        /// </summary>        
        public string? CadenaOriginal { get; set; }

        /// <summary>
        /// Cadena original del complemento de timbrado del CFDI
        /// </summary>        
        public string? CadenaOriginalComplemento { get; set; }

        /// <summary>
        /// Indica si se debe realizar el reintento del timbrado en el servicio de respaldo
        /// </summary>        
        public bool ReitentarTimbrado { get; set; }

        /// <summary>
        /// RFC del proveedor PAC con el que se timbro el CFDI
        /// </summary>        
        public required string RfcPac { get; set; }

        /// <summary>
        /// Folio Fiscal (UUID) del CFDI timbrado
        /// </summary>        
        public Guid? Uuid { get; set; }
    }
}
