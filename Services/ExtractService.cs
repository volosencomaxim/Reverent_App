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

        public JObject GetRequest(string rootURL, string dataContainer = null)
        {

            //Console.WriteLine(rootURL);
            var webClient = new WebClient();

            if (dataContainer != null)
                webClient.Headers.Add("X-Access-Token", dataContainer);
            
            var jsonString = webClient.DownloadString(rootURL);
            var entireJson = JObject.Parse(jsonString);

            return entireJson;
        }
    }
}
