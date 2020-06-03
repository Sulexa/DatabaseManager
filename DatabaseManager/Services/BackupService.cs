using DatabaseManager.Models;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace DatabaseManager.Services
{
    public class BackupService
    {
        private readonly BackupConfig _backupConfig;
        private readonly SqlConnection _sqlConnection;

        public BackupService(BackupConfig backupConfig, SqlConnection sqlConnection)
        {
            this._backupConfig = backupConfig;
            this._sqlConnection = sqlConnection;
        }

        public async Task Backup()
        {
            var sql = "BACKUP DATABASE @DatabaseName " +
                "TO DISK = @BackupPath " +
                "WITH COMPRESSION";

            SqlCommand command = new SqlCommand(sql, this._sqlConnection);

            command.CommandTimeout = 1200;

            command.Parameters.Add(new SqlParameter("@DatabaseName", this._backupConfig.DatabaseName));
            command.Parameters.Add(new SqlParameter("@BackupPath", this._backupConfig.BackupPath));

            await command.ExecuteNonQueryAsync();
        }
    }
}
