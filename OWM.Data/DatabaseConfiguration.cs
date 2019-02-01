using Microsoft.Extensions.Configuration;

namespace OWM.Data
{
    public class DatabaseConfiguration : ConfigurationBase
    {
        private string DataConnectionKey = nameof(OwmContext);

        public string GetDataConnectionString()
        {
            return base.GetConfiguration().GetConnectionString(DataConnectionKey);
        }
    }
}