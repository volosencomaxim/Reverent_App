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

            //Console.WriteLine(rootURL);

            if (dataContainer != null)
                webClient.Headers.Add("X-Access-Token", dataContainer);

            var entireJson = new JObject();
            try
            {
                var jsonString = webClient.DownloadString(rootURL);
                entireJson = JObject.Parse(jsonString);
                return entireJson;
            }
            catch (Exception e)
            {
            }
            return entireJson;
        }

        public void DataFilter(string key, string value)
        {
            string localS = "";

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
                StoredString = $"[{StoredString}]";
                var jsonArray = JArray.Parse(StoredString);
                return jsonArray;
            }
        }
        public JArray RefactoredJsonData()
        {
            var jsonData = ConvertDataToJArray();
            //Console.WriteLine(jsonData);
            return jsonData;
        }
    }
}
