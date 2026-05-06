namespace FacturacionElectronica.Net.Xml.Model.Common
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Tipos de dato soportados para propiedades dinámicas de interfaz ERP
    /// Supported data types for ERP interface dynamic properties
    /// </summary>
    public enum EnumPropiedadTipoDato : byte
    {
        String = 0,
        Int = 1,
        Double = 2,
        Decimal = 3,
        DateTime = 4,
        Bool = 5
    }

    /// <summary>
    /// Propiedad dinámica para interfaces ERP. Permite agregar campos personalizados
    /// sin modificar los DTOs base, soportando múltiples tipos de dato.
    /// Dynamic property for ERP interfaces. Allows adding custom fields
    /// without modifying base DTOs, supporting multiple data types.
    /// </summary>
    /// <example>
    /// PropiedadDinamica.CreateString("User1", "valor")
    /// PropiedadDinamica.CreateDouble("User3", 100.50)
    /// PropiedadDinamica.CreateDateTime("User7", DateTime.Now)
    /// </example>
    public class PropiedadDinamica
    {
        public PropiedadDinamica()
        {
            PropiedadId = string.Empty;
            TipoDato = EnumPropiedadTipoDato.String;
            Valor = string.Empty;
        }

        private PropiedadDinamica(string propiedadId, EnumPropiedadTipoDato tipoDato, string valor)
        {
            PropiedadId = propiedadId ?? throw new ArgumentNullException(nameof(propiedadId));
            TipoDato = tipoDato;
            Valor = valor;
        }

        #region Properties

        /// <summary>
        /// Identificador único de la propiedad (ej: "User1", "CampoPersonalizado")
        /// Unique property identifier (e.g.: "User1", "CustomField")
        /// </summary>
        public string PropiedadId { get; set; }

        /// <summary>
        /// Tipo de dato del valor almacenado
        /// Data type of the stored value
        /// </summary>        
        public EnumPropiedadTipoDato TipoDato { get; set; }

        /// <summary>
        /// Valor serializado como cadena de texto usando InvariantCulture.
        /// Use los métodos de fábrica (Create*) para asignar y los métodos tipados (Get*Value) para leer.
        /// Value serialized as string using InvariantCulture.
        /// Use factory methods (Create*) to assign and typed methods (Get*Value) to read.
        /// </summary>
        public string Valor { get; set; }

        #endregion

        #region Factory Methods

        public static PropiedadDinamica CreateString(string propiedadId, string valor)
        {
            return new PropiedadDinamica(propiedadId, EnumPropiedadTipoDato.String, valor ?? string.Empty);
        }

        public static PropiedadDinamica CreateInt(string propiedadId, int valor)
        {
            return new PropiedadDinamica(propiedadId, EnumPropiedadTipoDato.Int, valor.ToString(CultureInfo.InvariantCulture));
        }

        public static PropiedadDinamica CreateDouble(string propiedadId, double valor)
        {
            return new PropiedadDinamica(propiedadId, EnumPropiedadTipoDato.Double, valor.ToString(CultureInfo.InvariantCulture));
        }

        public static PropiedadDinamica CreateDecimal(string propiedadId, decimal valor)
        {
            return new PropiedadDinamica(propiedadId, EnumPropiedadTipoDato.Decimal, valor.ToString(CultureInfo.InvariantCulture));
        }

        public static PropiedadDinamica CreateDateTime(string propiedadId, DateTime valor)
        {
            return new PropiedadDinamica(propiedadId, EnumPropiedadTipoDato.DateTime, valor.ToString("o", CultureInfo.InvariantCulture));
        }

        public static PropiedadDinamica CreateBool(string propiedadId, bool valor)
        {
            return new PropiedadDinamica(propiedadId, EnumPropiedadTipoDato.Bool, valor.ToString(CultureInfo.InvariantCulture));
        }

        #endregion

        #region Typed Getters

        public string GetStringValue()
        {
            return Valor;
        }

        public int? GetIntValue()
        {
            if (int.TryParse(Valor, NumberStyles.Any, CultureInfo.InvariantCulture, out int result))
            {
                return result;
            }
            return null;
        }

        public double? GetDoubleValue()
        {
            if (double.TryParse(Valor, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            return null;
        }

        public decimal? GetDecimalValue()
        {
            if (decimal.TryParse(Valor, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }
            return null;
        }

        public DateTime? GetDateTimeValue()
        {
            if (DateTime.TryParse(Valor, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime result))
            {
                return result;
            }
            return null;
        }

        public bool? GetBoolValue()
        {
            if (bool.TryParse(Valor, out bool result))
            {
                return result;
            }
            return null;
        }

        #endregion

        public override string ToString()
        {
            return $"{PropiedadId} ({TipoDato}): {Valor}";
        }
    }
}
