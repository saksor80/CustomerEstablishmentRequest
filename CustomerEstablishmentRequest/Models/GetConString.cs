using Microsoft.Extensions.Configuration;
using System.IO;

namespace CustomerEstablishmentRequest.Models
{
    public class GetConString   //Sets GetConString class
    {
        public static string ConString()    //Gets sql connection string from appsettings.json-file
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);    //Set variable for path to .json-file
            var config = builder.Build();   //Sets variable for build config
            string constring = config.GetConnectionString("CustomerEstablishmentRequestContext");   //Sets string variable and gets sql connection string from appsettings.json-file to fill it
            return constring;   //Returns string
        }
    }
}
