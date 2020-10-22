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
                StoredString += dataParser.CSVToJson(value) + ",";

            if (key == "text/string")
                StoredString += dataParser.AdaptString(value) + ",";

            if (key == "application/x-yaml")
                StoredString += dataParser.XYAMToJson(value) + ",";

            if (key == "application/xml")
                StoredString += dataParser.XMLToJson(value) + ",";

        }

        private JArray ConvertDataToJArray()
        {
            var x = StoredString[StoredString.Length - 1];

            if (Equals(x , ','))
            {
                StoredString = StoredString.Remove(StoredString.Length - 1);
                return ConvertDataToJArray();
            }
            else
            {
                string arrayTypeString = $"[{StoredString}]";
                var jsonArray = JArray.Parse(arrayTypeString);
                return jsonArray;
            }
        }
        public void ShowRezult()
        {
            Console.WriteLine(StoredString);

            var smth = ConvertDataToJArray();
            Console.WriteLine(smth);
        }
    }
}
