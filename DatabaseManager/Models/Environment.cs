using DatabaseManager.Example;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DatabaseManager.Models
{
    public class Environment
    {
        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public DatabaseEnvironment DatabaseEnvironment { get; set; }
        [JsonRequired]
        public string DatabaseName { get; set; }
        [JsonRequired]
        public string DatabaseServerName { get; set; }
        [JsonRequired]
        public List<EnvironmentMigration> Migrations { get; set; }
    }
}
