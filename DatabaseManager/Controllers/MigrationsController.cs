using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DatabaseManager.Models;
using DatabaseManager.Services;
using Newtonsoft.Json;
using MigrationManager.Example;
using DatabaseManager.Example;

namespace DatabaseManager.Controllers
{
    [ApiController]
    [Route("migrations")]
    public class MigrationsController : ControllerBase
    {

        private readonly IContextFactoryServices<ExampleDbContext> _contextFactoryServices;

        public MigrationsController(IContextFactoryServices<ExampleDbContext> contextFactoryServices)
        {
            _contextFactoryServices = contextFactoryServices;
        }

        /// <summary>
        /// Api for user creation
        /// </summary>
        /// <returns>Return list of all environments</returns>
        /// <response code="200">User created</response>
        /// <response code="400">User creation failed</response>
        [HttpGet]
        [Route("list-all")]
        [ProducesResponseType(200, Type = typeof(List<Models.Environment>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetMigrationList()
        {
            var environments = new List<Models.Environment>();
            foreach (DatabaseEnvironment databaseEnvironment in (DatabaseEnvironment[])Enum.GetValues(typeof(DatabaseEnvironment)))
            {
                var migrationStatusService = this._contextFactoryServices.Get(databaseEnvironment);

                if (migrationStatusService == null)
                {
                    continue;
                }

                environments.Add(new Models.Environment
                {
                    Name = databaseEnvironment.ToString(),
                    DatabaseEnvironment = databaseEnvironment,
                    DatabaseName = migrationStatusService.GetDatabaseName(),
                    DatabaseServerName = migrationStatusService.GetDatabaseServerName(),
                    Migrations = await migrationStatusService.GetMigrationsAsync()
                });
            }
            return Ok(environments);
        }

        [HttpGet]
        [Route("list")]
        [ProducesResponseType(200, Type = typeof(List<EnvironmentMigration>))]
        public async Task<IActionResult> GetMigrations(DatabaseEnvironment environment)
        {
            return Ok(await this.GetMigrationsForEnvironment(environment));
        }

        [HttpPost]
        [Route("set")]
        [ProducesResponseType(200, Type = typeof(List<EnvironmentMigration>))]
        public async Task<IActionResult> SetMigration(DatabaseEnvironment environment, string migrationId)
        {
            await this._contextFactoryServices.Get(environment).MigrateAsync(migrationId);
            return Ok(await this.GetMigrationsForEnvironment(environment));
        }


        private async Task<List<EnvironmentMigration>> GetMigrationsForEnvironment(DatabaseEnvironment environment)
        {
            return await this._contextFactoryServices.Get(environment).GetMigrationsAsync();
        }

        [HttpGet]
        [Route("sql")]
        [ProducesResponseType(200, Type = typeof(SqlResult))]
        public IActionResult GetMigrationSql(DatabaseEnvironment environment, string fromMigrationId, string toMigrationId)
        {
            var sqlResult = new SqlResult
            {
                Content = this._contextFactoryServices.Get(environment).GenerateScript(fromMigrationId, toMigrationId)
            };
            return Ok(sqlResult);
        }

        public class SqlResult{
            [JsonRequired]
            public string Content { get; set; }
    }

    }
}
