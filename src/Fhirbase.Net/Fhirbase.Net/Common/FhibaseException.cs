using System;

namespace Fhirbase.Net.Common
{
    public class FhibaseException : Exception
    {
        public FhibaseException(string message) : base(message)
        {
        }

        public FhibaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
