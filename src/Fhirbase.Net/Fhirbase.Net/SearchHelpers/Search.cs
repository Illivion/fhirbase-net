using System;

namespace Fhirbase.Net.Helpers
{
    public static class Search
    {
        public static SearchParameters By(string key, string value)
        {
            var searchParameters = new SearchParameters();

            searchParameters.Parameters.Add(key, value);

            return searchParameters;
        }

        public static SearchParameters By(this SearchParameters parameters, string key, string value)
        {
            if (parameters.Parameters == null)
            {
                parameters.Parameters = System.Web.HttpUtility.ParseQueryString(String.Empty);
            }

            parameters.Parameters.Add(key, value);

            return parameters;
        }
    }
}