using System;

namespace DatabaseManager.Models
{
    public class RestoreConfig
    {
        public string RestoreDbName { get; set; }
        private string originalBackupPath;
        public string BackupPath {
            get
            {
                if (string.IsNullOrEmpty(originalBackupPath))
                {
                    return originalBackupPath;
                }
                return originalBackupPath.Replace("#date", this.BackupDate.ToString("yyyyMMdd"));
            }
            set {
                this.originalBackupPath = value;
            }
        }
        public string DatabaseFileName { get; set; }
        public string DatabaseFileNameRestorePath { get; set; }
        public string DatabaseLogFileName { get; set; }
        public string DatabaseLogFileNameRestorePath { get; set; }
        public DateTime BackupDate { get; set; } = DateTime.Now;
    }
}
