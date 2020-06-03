using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace DatabaseManager.Factories
{
    public class SqlDbContextFactory<T> : IDesignTimeDbContextFactory<T>
        where T:DbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        public SqlDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public SqlDbContextFactory(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        public T CreateDbContext()
        {
            return this.CreateDbContext(new string[0]);
        }

        public T CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();

            if (string.IsNullOrEmpty(_migrationAssembly))
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
            else
            {
                optionsBuilder.UseSqlServer(_connectionString, b => b.MigrationsAssembly(_migrationAssembly));
            }

            var dbContext = (T)Activator.CreateInstance(typeof(T), optionsBuilder.Options);

            return dbContext;
        }
    }
}
