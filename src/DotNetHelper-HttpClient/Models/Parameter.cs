using DotNetHelper_HttpClient.Enum;

namespace DotNetHelper_HttpClient.Models
{

    /// <summary>
    /// Parameter container for REST requests
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// Name of the parameter
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Value of the parameter
        /// </summary>
        /// <value>The value.</value>
        public object Value { get; set; }

        /// <summary>
        /// Type of the parameter
        /// </summary>
        /// <value>The type.</value>
        public ParameterType Type { get; set; } 

        /// <summary>
        /// MIME content type of the parameter
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; set; }

        public bool EscapeValue { get; set; }




        public Parameter(ParameterType type)
        {
            Type = type;
        }

        /// <summary>
        /// Return a human-readable representation of this parameter
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }


}
