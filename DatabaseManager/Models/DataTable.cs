using System.Collections.Generic;

namespace DatabaseManager.Models
{
    public class DataTable
    {
        public List<string> Columns { get; set; }

        public List<List<object>> Rows { get; set; }
    }
}
