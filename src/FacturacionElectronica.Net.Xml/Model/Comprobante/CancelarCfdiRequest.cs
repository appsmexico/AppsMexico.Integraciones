using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FacturacionElectronica.Net.Xml.Model.Comprobante
{
    /// <summary>
    /// Objeto para recibir la solicitu de cancelacion de un CFDI
    /// Object to get the cancellation request of a document.
    /// </summary>
    public class CancelarCfdiRequest
    {
        /// <summary>
        /// Id de la empresa que generó el comprobante.
        /// ID of the company that generated the document.
        /// </summary>
        [MaxLength(20)]        
        public required string EmpresaId { get; set; }

        /// <summary>
        /// Id del documento en el ERP. Es el número de lote o embarque del documento generado en el ERP.
        /// Id of the document on the ERP. Is the shipper or batch number of the document in the ERP.
        /// </summary>
        [MaxLength(50)]        
        public required string ErpId { get; set; }

        /// <summary>
        /// Fecha y Hora de la solicitud de cancelacion
        /// DateTime for cancellation request
        /// </summary>  
        public required DateTime FechaCancelado { get; set; }

        /// <summary>
        /// Comentarios del motivo de cancelación. Cuando se utiliza el portal de comprobantes se mustra el motivo en las solicitudes de cancelacion.
        /// Cancellation Reason Comments
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [MaxLength(1000)]       
        public string? MotivoCancelacion { get; set; }

        /// <summary>
        /// Codigo con el motivo de la cancelacion del CFDI
        /// </summary>
        public required string MotivoCancelacionCodigo { get; set; }

        /// <summary>
        /// Rfc del Receptor del CFDI que se quiere cancelar
        /// Customer Tax Id
        /// </summary>
        public required string RfcReceptor { get; set; }

        /// <summary>
        /// Sello digital del CFDI que se requiere cancelar
        /// CFDI Digital Signature
        /// </summary>
        public required string SelloDigital { get; set; }

        /// <summary>
        /// Total del CFDI
        /// Documento Total
        /// </summary>
        public required double Total { get; set; }

        /// <summary>
        /// Folio Fiscal (UUID) de la factura
        /// Document Digital Stamp
        /// </summary>
        public required Guid UUID { get; set; }

        /// <summary>
        /// Folio Fiscal (UUID) del CFDI que se esta sustituyendo
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]        
        public Guid? UUIDSustitucion { get; set; }
    }
}
