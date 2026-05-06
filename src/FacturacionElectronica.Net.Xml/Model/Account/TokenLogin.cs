namespace FacturacionElectronica.Net.Xml.Model.Account
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Clase para generar el response del inicio de sesión
    /// Class to generate the login response 
    /// </summary>
    public class TokenLogin
    {
        /// <summary>
        /// Fecha y hora en la que expira el token de inicio de sesión
        /// Date and time the login token expires 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]        
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Identificador del token de inicio de sesión
        /// </summary>        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]        
        public string? IdentityToken { get; set; }

        /// <summary>
        /// Indica si el usuario tiene o no confirmada su cuenta de correo en la aplicación cuando el método de autentificación es FormsAuthentication o Firebase
        /// Indicates whether or not the user has confirmed their email account in the application when the authentication method is FormsAuthentication or Firebase 
        /// </summary>        
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Indica si el usuario tiene o no confirmado su numero de celular en la aplicacion
        /// Indicates whether or not the user has their cell phone number confirmed in the application 
        /// </summary>        
        public bool IsPhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Mensaje de error cuando no se puede generar el token de inicio de sesión (OperacionExitosa = false)
        /// Error message when the login token cannot be generated (OperacionExitosa = false) 
        /// </summary>
        public string? Mensaje { get; set; }

        /// <summary>
        /// Indica si la generación del token fue exitosa o no
        /// Indicates if the token generation was successful or not 
        /// </summary>      
        public bool OperacionExitosa { get; set; }

        /// <summary>
        /// Token de actualización para generar un nuevo token una vez que el token ha expirado
        /// Refresh token to generate a new token once the token has expired
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]        
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Fecha en la que expira el token de actualización
        /// Date the refresh token expires
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]        
        public DateTime RefreshTokenExpiration { get; set; }

        /// <summary>
        /// Token para consumir la API
        /// Token to consume the API 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]        
        public string? Token { get; set; }

        /// <summary>
        /// Imagen del Avatar del Usuario en Base64
        /// Base64 User Avatar Image
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]        
        public string? AvatarBase64 { get; set; }
    }
}
