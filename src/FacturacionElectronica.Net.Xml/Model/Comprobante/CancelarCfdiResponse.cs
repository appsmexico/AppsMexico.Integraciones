namespace FacturacionElectronica.Net.Xml.Model.Comprobante
{
    using FacturacionElectronica.Net.Xml.Model.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Clase para enviar la respuesta de la cancelacion de un CFDI
    /// </summary>
    public class CancelarCfdiResponse : ApiResponse
    {
        public CancelarCfdiResponse() : base()
        {

        }

        public CancelarCfdiResponse(bool operacionExitosa) : base(operacionExitosa)
        {

        }

        /// <summary>
        /// Archivo en cadena Base64 del PDF con la representacion impresa del Acuse de cancelacion
        /// </summary>
        public string? ArchivoPdfBase64 { get; set; }


        /// <summary>
        /// Archivo en cadena Base64 del XML del acuse de cancelacion Encoding UTF8
        /// </summary>                
        public string? ArchivoXmlAcuseBase64 { get; set; }


        /// <summary>
        /// Archivo en cadena Base64 del XML de la solicitud de timbrado Encoding UTF8
        /// </summary>               
        public string? ArchivoXmlRequestBase64 { get; set; }

        /// <summary>
        /// Archivo en cadena Base64 del XML de la respusta de timbrado Encoding UTF8
        /// </summary>                
        public string? ArchivoXmlResponseBase64 { get; set; }

        /// <summary>
        /// Estatus de la consulta del CFDI en el SAT
        /// </summary>

        public EnumCodigoStatusSat? CodigoStatusSat { get; set; }

        /// <summary>
        /// Listado de folios fiscales relacionados al CFDI que se quiere cancelar
        /// </summary>        
        public List<CancelarCfdiDocumentoRelacionadoResponse>? DocumentosRelacionados { get; set; }

        /// <summary>
        /// Fecha en la que se realizo la solicitud de cancelacion
        /// </summary>
        public DateTime? FechaSolicitudCancelacion { get; set; }

        /// <summary>
        /// RFC del proveedor PAC con el que se cancelo el CFDI
        /// </summary>        
        public required string RfcPac { get; set; }

        /// <summary>
        /// Estatus de la solicitud de cancelacion
        /// </summary>        
        public EnumStatusCancelacion? StatusCancelacion { get; set; }

        /// <summary>
        /// Estatus para determinar si el CFDI es o no cancelable
        /// </summary>        
        public EnumStatusEsCancelable? StatusEsCancelable { get; set; }

        /// <summary>
        /// Folio Fiscal (UUID) del CFDI cancelado
        /// </summary>        
        public Guid Uuid { get; set; }
    }

    public enum EnumCodigoStatusSat
    {
        [Description("S - Comprobante obtenido satisfactoriamente.Vigente")]
        Vigente = 0,

        [Description("S - Comprobante obtenido satisfactoriamente.Cancelado")]
        Cancelado = 1,

        [Description("N - 602: Comprobante no encontrado.No Encontrado")]
        NoEncontrado = 3,

        [Description("N - 601: La expresión impresa proporcionada no es válida.No Encontrado")]
        ExpresionInvalida = 4
    }

    public enum EnumStatusCancelacion
    {
        [Description("Cancelado con aceptación")]
        Cancelado_con_Aceptacion = 0,

        [Description("Cancelado sin aceptación")]
        Cancelado_sin_Aceptacion = 1,

        [Description("Cancelado Plazo Vencido")]
        Cancelado_Plazo_Vencido = 2,

        [Description("En proceso")]
        En_Proceso = 3,

        [Description("No cancelable")]
        No_Cancelable = 4,

        [Description("Solicitud rechazada")]
        Solicitud_Rechazada = 5,
    }

    public enum EnumStatusEsCancelable
    {
        [Description("Cancelable con aceptación")]
        Cancelable_con_Aceptacion = 0,

        [Description("Cancelable sin aceptación")]
        Cancelable_sin_Aceptacion = 1,

        [Description("No cancelable")]
        No_Cancelable = 2,
    }
}
