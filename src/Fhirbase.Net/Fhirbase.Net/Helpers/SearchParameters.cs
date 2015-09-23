using System;
using System.Collections.Specialized;

namespace Fhirbase.Net.Helpers
{
    public class SearchParameters
    {
        public static readonly SearchParameters Empty = new SearchParameters();

        public SearchParameters()
        {
            Parameters = System.Web.HttpUtility.ParseQueryString(String.Empty);
        }

        public NameValueCollection Parameters { get; set; }

        /// <summary>
        /// Возвращает форматированную строку параметров
        /// </summary>
        public override string ToString()
        {
            return Parameters?.ToString() ?? String.Empty;
        }
    }
}
