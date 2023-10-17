using Microsoft.Extensions.Configuration;
using System.IO;

namespace TryPKI.SqlConnector
{
    public class Connection
    {
        private IConfigurationRoot Configuration { get; set; }
        public string GetConnectionString()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = configuration.Build();
            string constr1 = Configuration.GetConnectionString("myconnection");
            return constr1;
        }
    }
}
