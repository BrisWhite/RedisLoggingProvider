using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.RedisProvider
{
    public class RedisLoggingConfiguration
    {
        public string ConnectionString { get; set; }

        public int Database { get; set; }

        public string ProjectName { get; set; }

        public RedisLoggingConfiguration() { }

        public void SetConfiguration(string connectionString, int db, string projectName) 
        {
            ConnectionString = connectionString;
            Database = db;
            ProjectName = projectName;
        }

        public bool IsValid
        {
            get
            {
                return Validate();
            }
        }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                throw new ArgumentException(nameof(ConnectionString));
            }

            if (Database < 0)
            {
                throw new ArgumentException(nameof(Database));
            }

            if (string.IsNullOrWhiteSpace(ProjectName))
            {
                throw new ArgumentException(nameof(ProjectName));
            }

            return true;
        }
    }
}
