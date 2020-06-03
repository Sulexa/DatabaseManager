using System;
using System.Threading.Tasks;
using DatabaseManager.Example;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MigrationManager.Example;
using DatabaseManager.Models;
using DatabaseManager.Services;

namespace DatabaseManager.Controllers
{
    [ApiController]
    [Route("scripts")]
    public class ScriptsController : ControllerBase
    {

        private readonly IContextFactoryServices<ExampleDbContext> _contextFactoryServices;
        private readonly IConfiguration _configuration;

        public ScriptsController(IConfiguration configuration, IContextFactoryServices<ExampleDbContext> contextFactoryServices)
        {
            _contextFactoryServices = contextFactoryServices;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("clean")]
        [ProducesResponseType(200, Type = typeof(void))]
        public async Task<IActionResult> CleanDatabase(DatabaseEnvironment environment)
        {
            if (MigrationConfiguration.CurrentEnvironment == DatabaseEnvironment.PROD)
            {
                throw new Exception("This script is not allowed to run in production");
            }
            var dbContext = this._contextFactoryServices.GetDbContextFromDatabaseEnvironment(environment);
            await this.CleanDatabase(environment, dbContext);
            return Ok();
        }

        private async Task CleanDatabase(DatabaseEnvironment environment, ExampleDbContext dbContext)
        {
            MigrationConfiguration.CurrentEnvironment = environment;
            throw new NotImplementedException("No generic clean");
        }

        [HttpPost]
        [Route("restore")]
        [ProducesResponseType(200, Type = typeof(void))]
        public async Task<IActionResult> RestoreDatabase(DatabaseEnvironment databaseEnvironment)
        {
            if (MigrationConfiguration.CurrentEnvironment == DatabaseEnvironment.PROD)
            {
                throw new Exception("This script is not allowed to run in production");
            }
            var dbContext = this._contextFactoryServices.GetDbContextFromDatabaseEnvironment(databaseEnvironment);

            var restoreConfig = new RestoreConfig();
            this._configuration.GetSection($"RestoreConfigs:{databaseEnvironment}").Bind(restoreConfig);

            if (restoreConfig == null)
            {
                throw new Exception("No restore config for environment");
            }

            var connectionString = _configuration.GetConnectionString(databaseEnvironment.ToString());
            connectionString = this.RemoveDatabasePartFromConnectionString(connectionString);

            var sqlConnection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);

            sqlConnection.Open();

            if(await SetBackupDayAsync(sqlConnection, restoreConfig) == false)
            {
                throw new Exception("Backups not found");
            }

            var restorationService = new RestorationService(restoreConfig, sqlConnection);

            await restorationService.DropIfExistAsync();

            await restorationService.RestoreAsync();

            sqlConnection.Close();

            await this.CleanDatabase(databaseEnvironment, dbContext);

            return Ok();
        }

        private async Task<bool> SetBackupDayAsync(Microsoft.Data.SqlClient.SqlConnection sqlConnection, RestoreConfig restoreConfig)
        {
            var sqlServerService = new SqlServerService(sqlConnection);

            var currentDayBackupExist = await sqlServerService.FileExistAsync(restoreConfig.BackupPath);

            if (currentDayBackupExist == false)
            {
                restoreConfig.BackupDate = restoreConfig.BackupDate.AddDays(-1);//On check la backup d'hier
                var previousDayBackupExist = await sqlServerService.FileExistAsync(restoreConfig.BackupPath);

                if (previousDayBackupExist == false)
                {
                    return false;
                }
            }

            return true;
        }



        [HttpPost]
        [Route("backup")]
        [ProducesResponseType(200, Type = typeof(void))]
        public async Task<IActionResult> BackupDatabase(DatabaseEnvironment databaseEnvironment)
        {
            var backupConfig = new BackupConfig();
            this._configuration.GetSection($"BackupConfigs:{databaseEnvironment.ToString()}").Bind(backupConfig);

            if (backupConfig == null)
            {
                throw new Exception("No backup config for environment");
            }

            var connectionString = _configuration.GetConnectionString(databaseEnvironment.ToString());

            var sqlConnection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
            sqlConnection.Open();

            var backupService = new BackupService(backupConfig, sqlConnection);

            await backupService.Backup();

            sqlConnection.Close();


            return Ok();
        }


        private string RemoveDatabasePartFromConnectionString(string connectionString)
        {
            var startIndexOf = connectionString.IndexOf(";initial catalog=");
            var endIndexOf = connectionString.IndexOf(";", startIndexOf + 1);
            connectionString = connectionString.Remove(startIndexOf, endIndexOf - startIndexOf);
            return connectionString;
        }
    }
}
