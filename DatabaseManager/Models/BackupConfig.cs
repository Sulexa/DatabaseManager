using System;

namespace DatabaseManager.Models
{
    public class BackupConfig
    {
        public string DatabaseName { get; set; }
        private string originalBackupPath;
        public string BackupPath
        {
            get
            {
                if (string.IsNullOrEmpty(originalBackupPath))
                {
                    return originalBackupPath;
                }
                return originalBackupPath.Replace("#full_date", DateTime.Now.ToString("yyyyMMdd_hhmmss"));
            }
            set
            {
                this.originalBackupPath = value;
            }
        }
    }
}
