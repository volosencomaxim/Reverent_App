using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Reverent_App.Services
{


    
    public class ExtractService
    {

        private string StoredString;
        private DataParser dataParser;

        public ExtractService()
        {
            StoredString = "";
            dataParser = new DataParser();
        }

        public JObject GetRequest(string rootURL, string dataContainer = null)
        {
            var webClient = new WebClient();

            Console.WriteLine(rootURL);

            if (dataContainer != null)
                webClient.Headers.Add("X-Access-Token", dataContainer);
            
            var jsonString = webClient.DownloadString(rootURL);
            var entireJson = JObject.Parse(jsonString);

            return entireJson;
        }

        public void DataFilter(string key, string value)
        {
            string localS = "";


            //Console.WriteLine(key + "   :    " + value);

            if (key == "text/csv")
            {
                localS = dataParser.CSVToJson(value);
                StoredString += localS + ",";
            }
            if (key == "text/string")
            {
                localS = dataParser.AdaptString(value);
                StoredString += localS + ",";
            }
            if (key == "application/x-yaml")
            {
                localS = dataParser.XYAMToJson(value);
                StoredString += localS + ",";
            }
            if (key == "application/xml")
            {
                localS = dataParser.XMLToJson(value);
                StoredString += localS + ",";
            }
            //Console.WriteLine(StoredString + "\n\n\n LAST ONE   ");
        }

        public void ShowRezult()
        {
            Console.WriteLine(StoredString);
        }
    }
}
