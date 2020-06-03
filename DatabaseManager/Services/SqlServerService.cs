using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace DatabaseManager.Services
{
    public interface ISqlServerService
    {
        Task<bool> FileExistAsync(string filePath);
    }

    public class SqlServerService : ISqlServerService
    {
        private readonly SqlConnection _sqlConnection;
        public SqlServerService(SqlConnection sqlConnection)
        {
            this._sqlConnection = sqlConnection;
        }

        public async Task<bool> FileExistAsync(string filePath)
        {
            SqlCommand command = new SqlCommand("master.sys.xp_fileexist", this._sqlConnection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var existParameter = new SqlParameter
            {
                ParameterName = "@exist",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };


            command.Parameters.Add(new SqlParameter("@filePath", filePath));
            command.Parameters.Add(existParameter);

            await command.ExecuteNonQueryAsync();

            bool exist = Convert.ToBoolean(existParameter.Value);

            return exist;
        }
    }
}
