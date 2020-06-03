using DatabaseManager.Example;
using Microsoft.EntityFrameworkCore;
using DatabaseManager.Factories;
using DatabaseManager.Models;
using System.Collections.Generic;

namespace DatabaseManager.Services
{

    public interface IContextFactoryServices<TDbContext> where TDbContext: DbContext
    {
        IMigrationStatusService Get(DatabaseEnvironment databaseEnvironment);
        TDbContext GetDbContextFromDatabaseEnvironment(DatabaseEnvironment databaseEnvironment);
    }

    public class ContextFactoryServices<TDbContext> : IContextFactoryServices<TDbContext> where TDbContext : DbContext
    {
        private readonly Dictionary<DatabaseEnvironment, IMigrationStatusService> _migrationStatusServiceDict;
        private readonly Dictionary<DatabaseEnvironment, TDbContext> _dbContextDict;
        private readonly string _migrationAssembly;

        public ContextFactoryServices(IEnumerable<DatabaseConfig> databaseDatas)
        {
            this._migrationStatusServiceDict = new Dictionary<DatabaseEnvironment, IMigrationStatusService>();
            this._dbContextDict = new Dictionary<DatabaseEnvironment, TDbContext>();

            foreach (var databaseData in databaseDatas)
            {
                this.AddDbContext(databaseData);
                this.AddDatabaseEnvironmentMigrationStatusServices(databaseData);
            }
        }

        public ContextFactoryServices(IEnumerable<DatabaseConfig> databaseDatas, string migrationAssembly)
        {
            _migrationAssembly = migrationAssembly;
            this._migrationStatusServiceDict = new Dictionary<DatabaseEnvironment, IMigrationStatusService>();
            this._dbContextDict = new Dictionary<DatabaseEnvironment, TDbContext>();

            foreach (var databaseData in databaseDatas)
            {
                this.AddDbContext(databaseData);
                this.AddDatabaseEnvironmentMigrationStatusServices(databaseData);
            }
        }
        public TDbContext GetDbContextFromDatabaseEnvironment(DatabaseEnvironment databaseEnvironment)
        {
            return this._dbContextDict[databaseEnvironment];
        }

        private void AddDbContext(DatabaseConfig databaseConfig)
        {
            this._dbContextDict.Add(databaseConfig.DatabaseEnvironment, this.GetDbContextFromConnectionString(databaseConfig.ConnectionString));
        }

        private TDbContext GetDbContextFromConnectionString(string connectionString)
        {
            var sqlDbContextFactory = new SqlDbContextFactory<TDbContext>(connectionString, this._migrationAssembly);

            return sqlDbContextFactory.CreateDbContext();
        }

        private void AddDatabaseEnvironmentMigrationStatusServices(DatabaseConfig databaseConfig)
        {
            var dbContext = this._dbContextDict[databaseConfig.DatabaseEnvironment];

            var migrationStatusServices = new MigrationStatusServices(dbContext);
            this._migrationStatusServiceDict.Add(databaseConfig.DatabaseEnvironment, migrationStatusServices);

        }

        public IMigrationStatusService Get(DatabaseEnvironment databaseEnvironment)
        {
            if (!this._migrationStatusServiceDict.ContainsKey(databaseEnvironment))
            {
                return null;
            }
            return this._migrationStatusServiceDict[databaseEnvironment];
        }
    }
}
