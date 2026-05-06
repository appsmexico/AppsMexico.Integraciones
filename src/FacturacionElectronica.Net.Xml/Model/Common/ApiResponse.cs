namespace FacturacionElectronica.Net.Xml.Model.Common
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Objeto enviar la respuesta de un proceso
    /// Object to send the response of a process 
    /// </summary>
    public class ApiResponse
    {
        public ApiResponse() 
        {
            this.OperacionExitosa = false;
        }
        
        public ApiResponse(bool operacionExitosa)
        {
            this.OperacionExitosa = operacionExitosa;
        }

        /// <summary>
        /// ID o Codigo de Mensaje de la respuesta
        /// ID or Message Code of the response
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// ID relacionado con la transacción realizada, por ejemplo el ID del documento creado o actualizado en el proceso de la interfaz, para poder tener una referencia del resultado de la transacción realizada en el proceso.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? IdRelacionado { get; set; }

        /// <summary>
        /// Mensaje de la respuesta
        /// Response Message
        /// </summary>
        public required string Descripcion { get; set; }

        /// <summary>
        /// Indica si el proceso se ejecuto exitosamente o no. True para respuestas exitosas y Falso para respuestas erroneas
        /// True for successful response and False for wrong response
        /// </summary>
        public bool OperacionExitosa { get; set; }

        /// <summary>
        /// Lista de mensajes para permitir la respuesta de procesos con multiples transacciones
        /// List of messages to allow the response of processes with multiple transactions 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Mensaje>? Mensajes { get; set; }

        /// <summary>
        /// Lista de parametros para enviar en la respuesta.
        /// List of parameters to send in response 
        /// NOTA: Esta propiedad se mantiene para aplicaciones legacy, para nuevas implementaciones se recomienda utilizar la propiedad PropiedadesDinamicas para enviar campos personalizados en la respuesta de cada proceso.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Parametro>? Parametros { get; set; }

        /// <summary>
        /// Propiedades dinámicas para campos personalizados de cada ERP (ej: User1..User9 en Dynamics SL)
        /// Dynamic properties for custom fields per ERP (e.g.: User1..User9 in Dynamics SL)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<PropiedadDinamica>? PropiedadesDinamicas { get; set; }
    }
}
