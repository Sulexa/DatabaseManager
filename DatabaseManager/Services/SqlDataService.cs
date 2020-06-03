using Microsoft.Data.SqlClient;
using DatabaseManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseManager.Services
{
    public interface ISqlDataService
    {
        Task<List<string>> GetDataTables();

        Task<DataTable> GetDataTableContent(string tableName);
    }

    public class SqlDataService : ISqlDataService
    {
        private readonly SqlConnection _sqlConnection;

        public SqlDataService(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<List<string>> GetDataTables()
        {
            var sql = @"SELECT
	                        TABLE_NAME
                        FROM
  	                        INFORMATION_SCHEMA.TABLES
                        WHERE
	                        TABLE_SCHEMA = 'data'
                        ORDER BY
                            TABLE_NAME ASC";

            var tables = new List<string>();

            _sqlConnection.Open();

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandTimeout = 3600;

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        tables.Add((string)reader.GetValue(0));
                    }
                }
            }

            _sqlConnection.Close();

            return tables;
        }

        public async Task<DataTable> GetDataTableContent(string tableName)
        {
            var sql = @$"SELECT
	                        *
                        FROM
  	                        data.{tableName}";

            var table = new DataTable()
            {
                Columns = new List<string>(),
                Rows = new List<List<object>>()
            };

            _sqlConnection.Open();

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandTimeout = 3600;

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        table.Columns.Add(reader.GetName(i));
                    }

                    while (reader.Read())
                    {
                        var row = new List<object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (reader.IsDBNull(i))
                            {
                                row.Add("NULL");
                            }
                            else
                            {
                                row.Add(reader.GetValue(i));
                            }
                        }

                        table.Rows.Add(row);
                    }
                }
            }

            _sqlConnection.Close();

            return table;
        }
    }
}