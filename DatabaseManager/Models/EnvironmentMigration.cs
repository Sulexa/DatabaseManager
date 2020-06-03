using Newtonsoft.Json;
using System;

namespace DatabaseManager.Models
{

    public class EnvironmentMigration
    {
        [JsonRequired]
        public string Id { get; }
        [JsonRequired]
        public string Name { get; }
        [JsonRequired]
        public DateTime Date { get; }
        [JsonRequired]
        public bool Applied { get; }
        public EnvironmentMigration(string id, string name, DateTime date, bool applied)
        {
            this.Id = id;
            this.Name = name.Replace('_', ' ');
            this.Date = date;
            this.Applied = applied;
        }
    }
}
