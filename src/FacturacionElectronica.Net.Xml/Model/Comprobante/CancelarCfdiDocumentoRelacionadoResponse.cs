namespace FacturacionElectronica.Net.Xml.Model.Comprobante
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Clase para enviar la respuesta con los datos de un documento relacionado en la cancelacion de un CFDI
    /// </summary>
    public class CancelarCfdiDocumentoRelacionadoResponse
    {
        /// <summary>
        /// Estatus de la consulta del CFDI relacionado en el SAT
        /// </summary>       
        public string CodigoStatusSat { get; set; }

        /// <summary>
        /// Mensaje del CFDI relacionado
        /// </summary>        
        public string Mensaje { get; set; }

        /// <summary>
        /// Registro Federal de Contribuyentes (RFC) del Emisor del CFDI
        /// </summary>
        [RegularExpression(@"[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A-Z]?")]
        [StringLength(13, MinimumLength = 12)]        
        public string RfcEmisor { get; set; }

        /// <summary>
        /// Registro Federal de Contribuyentes (RFC) del Receptor del CFDI
        /// </summary>
        [RegularExpression(@"[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A-Z]?")]
        [StringLength(13, MinimumLength = 12)]
        
        public string RfcReceptor { get; set; }

        /// <summary>
        /// Tipo de Relacion de los documentos (Padres o Hijos)
        /// </summary>        
        public string TipoRelacion { get; set; }

        /// <summary>
        /// Folio Fiscal (UUID) del CFDI relacionado
        /// </summary>        
        public Guid Uuid { get; set; }
    }
}
