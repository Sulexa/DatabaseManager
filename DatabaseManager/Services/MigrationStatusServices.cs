using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using DatabaseManager.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManager.Services
{

    public interface IMigrationStatusService
    {
        string GenerateScript(string fromMigrationId, string toMigrationId);
        Task<List<EnvironmentMigration>> GetMigrationsAsync();
        Task MigrateAsync(string migrationId);
        string GetDatabaseName();
        string GetDatabaseServerName();
    }

    public class MigrationStatusServices : IMigrationStatusService
    {
        private readonly DbContext _dbContext;
        private readonly IMigrator _migrator;
        public MigrationStatusServices(DbContext dbContext)
        {
            this._dbContext = dbContext;
            _migrator = dbContext.Database.GetInfrastructure().GetService<IMigrator>();
        }

        public async Task MigrateAsync(string migrationId)
        {
            await _migrator.MigrateAsync(migrationId);
        }
        public string GenerateScript(string fromMigrationId, string toMigrationId)
        {
            return _migrator.GenerateScript(fromMigrationId, toMigrationId);
        }

        public async Task<List<EnvironmentMigration>> GetMigrationsAsync()
        {
            var migrations = new List<EnvironmentMigration>();

            var pendingMigrations = await this._dbContext.Database.GetPendingMigrationsAsync();
            migrations.AddRange(pendingMigrations.Select(pm => GetMigrationFromEFCoreMigrationName(pm, false)));

            var appliedMigrations = await this._dbContext.Database.GetAppliedMigrationsAsync();
            migrations.AddRange(appliedMigrations.Select(am => GetMigrationFromEFCoreMigrationName(am, true)));

            return migrations.OrderBy(m => m.Date).ToList();
        }

        private EnvironmentMigration GetMigrationFromEFCoreMigrationName(string migrationId, bool applied)
        {
            var splitedDatas = migrationId.Split('_', 2);

            string formatString = "yyyyMMddHHmmss";
            var date = DateTime.ParseExact(splitedDatas[0], formatString, CultureInfo.InvariantCulture);

            var name = splitedDatas[1];

            return new EnvironmentMigration(migrationId, name, date, applied);
        }

        public string GetDatabaseName()
        {
            return this._dbContext.Database.GetDbConnection().Database;
        }

        public string GetDatabaseServerName()
        {
            return this._dbContext.Database.GetDbConnection().DataSource;
        }
    }
}
