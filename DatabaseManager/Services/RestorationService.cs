using DatabaseManager.Models;
using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace DatabaseManager.Services
{
    public class RestorationService
    {
        private readonly RestoreConfig _restoreConfig;
        private readonly SqlConnection _sqlConnection;

        public RestorationService(RestoreConfig restoreConfig, SqlConnection sqlConnection)
        {
            this._restoreConfig = restoreConfig;
            this._sqlConnection = sqlConnection;
        }

        public async Task RestoreAsync()
        {
            var sql = "RESTORE DATABASE @RestoreDbName " +
                "FROM DISK = @BackupPath " +
                "WITH RECOVERY, " +
                "MOVE @DatabaseFileName TO @DatabaseFileNameRestorePath, " +
                "MOVE @DatabaseLogFileName TO @DatabaseLogFileNameRestorePath";


            SqlCommand command = new SqlCommand(sql, this._sqlConnection);

            command.CommandTimeout = 3600;

            command.Parameters.Add(new SqlParameter("@RestoreDbName", this._restoreConfig.RestoreDbName));
            command.Parameters.Add(new SqlParameter("@BackupPath", this._restoreConfig.BackupPath));

            command.Parameters.Add(new SqlParameter("@DatabaseFileName", this._restoreConfig.DatabaseFileName));
            command.Parameters.Add(new SqlParameter("@DatabaseFileNameRestorePath", this._restoreConfig.DatabaseFileNameRestorePath));

            command.Parameters.Add(new SqlParameter("@DatabaseLogFileName", this._restoreConfig.DatabaseLogFileName));
            command.Parameters.Add(new SqlParameter("@DatabaseLogFileNameRestorePath", this._restoreConfig.DatabaseLogFileNameRestorePath));

            await command.ExecuteNonQueryAsync();
        }

        public async Task DropIfExistAsync()
        {
            var exist = await this.CheckExist();
            if(exist == false)
            {
                return;
            }

            await this.DropAsync();
        }

        public async Task<bool> CheckExist()
        {
            var sql = "IF DB_ID(@RestoreDbName) IS NOT NULL SET @exist = 1 ELSE SET @exist = 0";
            SqlCommand command = new SqlCommand(sql, this._sqlConnection);
            command.Parameters.Add(new SqlParameter("@RestoreDbName", this._restoreConfig.RestoreDbName));
            var existParameter = new SqlParameter
            {
                ParameterName = "@exist",
                SqlDbType = System.Data.SqlDbType.Bit,
                Direction = System.Data.ParameterDirection.Output
            };
            command.Parameters.Add(existParameter);
            await command.ExecuteNonQueryAsync();

            bool exist = Convert.ToBoolean(existParameter.Value);

            return exist;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Data in config, can't use sqlparameter for drop")]
        public async Task DropAsync()
        {
            var sql = "USE master;" +
                $"ALTER DATABASE {this._restoreConfig.RestoreDbName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;" +
                $"DROP DATABASE  {this._restoreConfig.RestoreDbName};";

            SqlCommand command = new SqlCommand(sql, this._sqlConnection);
            await command.ExecuteNonQueryAsync();
        }
    }
}
