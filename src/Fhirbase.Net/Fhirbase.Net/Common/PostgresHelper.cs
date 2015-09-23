using System;
using System.Configuration;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace Fhirbase.Net.Common
{
    internal class PostgresHelper
    {
        public static object Function(string connectionString, string functionName, params NpgsqlParameter[] parameters)
        {
            var npgsqlConnection = OpenConnection(connectionString);

            try
            {
                var command = new NpgsqlCommand(functionName, npgsqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddRange(parameters);
                
                var result = command.ExecuteScalar();

                return result;
            }
            catch (Exception ex)
            {
                throw new FhibaseException(
                    string.Format("Call {0} FHIRbase function failed. Reason {1}", functionName, ex.Message),
                    ex);
            }
            finally
            {
                npgsqlConnection.Close();
            }
        }

        private static NpgsqlConnection OpenConnection(string connectionString)
        {
            var npgsqlConnection = new NpgsqlConnection(connectionString);

            npgsqlConnection.Open();

            return npgsqlConnection;
        }

        public static NpgsqlParameter TextParam(string text)
        {
            return new NpgsqlParameter
            {
                NpgsqlDbType = NpgsqlDbType.Text,
                Value = text,
            };
        }

        public static NpgsqlParameter Jsonb(string text)
        {
            return new NpgsqlParameter
            {
                NpgsqlDbType = NpgsqlDbType.Jsonb,
                Value = text,
            };
        }

        public static NpgsqlParameter StringArray(string[] resources)
        {
            return new NpgsqlParameter
            {
                NpgsqlDbType = NpgsqlDbType.Array,
                Value = resources,
            };
        }

        public static NpgsqlParameter Int(int limit)
        {
            return new NpgsqlParameter
            {
                NpgsqlDbType = NpgsqlDbType.Integer,
                Value = limit,
            };
        }
    }
}
