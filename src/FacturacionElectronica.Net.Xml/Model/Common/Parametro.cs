namespace FacturacionElectronica.Net.Xml.Model.Common
{
    public class Parametro
    {
        /// <summary>
        /// Id o Código del parámetro
        /// Parameter ID or Code
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Valor del parámetro
        /// Parameter value
        /// </summary>
        public string? Valor { get; set; }
    }
}
