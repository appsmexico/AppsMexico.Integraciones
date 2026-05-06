using Newtonsoft.Json;
using System.ComponentModel;

namespace FacturacionElectronica.Net.Xml.Model.Common
{
    /// <summary>
    /// Identifica el tipo de mensaje
    /// Identify the type of message 
    /// </summary>
    public enum EnumTipoMensaje
    {
        /// <summary>
        /// Mensaje Exitoso o Informativo
        /// Successful or Informational Message 
        /// </summary>
        [Description("Mensaje de caracter normal que muestra informacion descriptiva del proceso.")]
        Informacion = 0,

        /// <summary>
        /// Mensaje con Error
        /// Error Message 
        /// </summary>
        [Description("Mensaje que indica la existencia de un error en el proceso.")]
        Error = 1,

        /// <summary>
        /// Mensaje de Advertencia
        /// Warning message 
        /// </summary>
        [Description("Mensaje que indica la existencia de un advertencia en el proceso.")]
        Advertencia = 2,
    };

    public class Mensaje
    {
        private EnumTipoMensaje _Tipo;

        /// <summary>
        /// Tipo de Mensaje Informacion, Error, Advertencia 
        /// Type of Message Information, Error, Warning 
        /// </summary>
        public required EnumTipoMensaje Tipo
        {
            get { return _Tipo; }
            set
            {
                _Tipo = value;
                if (_Tipo == EnumTipoMensaje.Error)
                {
                    OperacionExitosa = false;
                }
                else
                {
                    OperacionExitosa = true;
                }
            }
        }

        /// <summary>
        /// Descripcion del mensaje
        /// Message description 
        /// </summary>
        public required string Descripcion { get; set; }

        /// <summary>
        /// Id o Codigo del mensaje
        /// Message ID or Code 
        /// </summary>
        public string? Id { get; set; }

        private bool _OperacionExitosa;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool OperacionExitosa
        {
            get
            {
                return _OperacionExitosa;
            }
            set
            {
                _OperacionExitosa = value;

                if (_OperacionExitosa == false && _Tipo != EnumTipoMensaje.Error)
                {
                    Tipo = EnumTipoMensaje.Error;
                }
            }
        }
    }
}
