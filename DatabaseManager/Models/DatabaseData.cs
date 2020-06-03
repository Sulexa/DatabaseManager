using DatabaseManager.Example;

namespace DatabaseManager.Models
{
    public class DatabaseConfig
    {
        public DatabaseEnvironment DatabaseEnvironment { get; set; }
        public string ConnectionString { get; set; }
    }
}
