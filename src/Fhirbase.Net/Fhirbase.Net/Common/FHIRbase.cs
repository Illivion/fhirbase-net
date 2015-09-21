using System.Collections.Generic;
using Npgsql;

namespace Fhirbase.Net.Common
{
    public static class FHIRbase
    {
        /// <summary>
        /// Create FHIRbase function
        /// </summary>
        /// <param name="fhirbaseFunc"></param>
        /// <returns></returns>
        public static FhirBaseFunction Call(string fhirbaseFunc)
        {
            return new FhirBaseFunction {Name = fhirbaseFunc};
        }

        public static FhirBaseFunction Call(string schema, string fhirbaseFunc)
        {
            return new FhirBaseFunction {Name = schema + "." + fhirbaseFunc};
        }

        /// <summary>
        /// Add text parameter
        /// </summary>
        /// <param name="func"></param>
        /// <param name="textParameter"></param>
        /// <returns></returns>
        public static FhirBaseFunction WithText(this FhirBaseFunction func, string textParameter)
        {
            func.Parameters.Add(PostgresHelper.Text(textParameter));
            return func;
        }

        /// <summary>
        /// Add jsonb parameter
        /// </summary>
        /// <param name="func"></param>
        /// <param name="jsonParameter"></param>
        /// <returns></returns>
        public static FhirBaseFunction WithJsonb(this FhirBaseFunction func, string jsonParameter)
        {
            func.Parameters.Add(PostgresHelper.Jsonb(jsonParameter));
            return func;
        }

        public static FhirBaseFunction WithTextArray(this FhirBaseFunction func, string[] resources)
        {
            func.Parameters.Add(PostgresHelper.TextArray(resources));
            return func;
        }

        public static FhirBaseFunction WithInt(this FhirBaseFunction func, int limit)
        {
            func.Parameters.Add(PostgresHelper.Int(limit));
            return func;
        }

        /// <summary>
        /// Call FHIRbase function and cast value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T Cast<T>(this FhirBaseFunction func)
        {
            return (T)PostgresHelper.Func(func.Name, func.Parameters.ToArray());
        }

        public class FhirBaseFunction
        {
            public FhirBaseFunction()
            {
                Parameters = new List<NpgsqlParameter>();
            }

            public string Name { get; set; }

            public List<NpgsqlParameter> Parameters { get; set; }
        }
    }
}
