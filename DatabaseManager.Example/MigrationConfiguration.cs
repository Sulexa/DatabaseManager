namespace DatabaseManager.Example
{
    public enum DatabaseEnvironment
    {
        LOCAL,
        DEV,
        INT,
        UAT,
        PROD
    }
    public static class MigrationConfiguration
    {
        public static DatabaseEnvironment CurrentEnvironment { get; set; }
    }

}
