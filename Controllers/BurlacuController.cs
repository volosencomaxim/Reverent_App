using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Reverent_App.Services;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace Reverent_App.Controllers
{
    class BurlacuController
    {
        string rootURL;
        string accessToken;


        ExtractService extractService;
        JObject dataConrainer;


        public BurlacuController()
        {
            rootURL = Global.AppSetUp.RootURL + "/register";
            extractService = new ExtractService();
            
            dataConrainer = extractService.GetRequest(rootURL);
            accessToken = dataConrainer["access_token"].ToString();
        }
        public JObject HomeRoute()
        {

            string nextLink = Global.AppSetUp.RootURL + dataConrainer["link"].ToString();

            var firstJson = extractService.GetRequest(nextLink, accessToken);

            return firstJson;
        }


        public void DemoStart()
        {

            string nextLink = Global.AppSetUp.RootURL + dataConrainer["link"].ToString();

            var firstJson = extractService.GetRequest(nextLink, accessToken);

            GetRequest(firstJson);
            //Console.WriteLine(token);
            //Console.WriteLine(nextLink);

        }

        public void GetRequest(JObject entireJson)
        {

            if (entireJson["link"] != null)
            {
                var linkJson = JObject.Parse(entireJson["link"].ToString());

                foreach (JProperty property in linkJson.Properties())
                {
                    string nextLink = Global.AppSetUp.RootURL + property.Value;

                    //Console.WriteLine(nextLink);

                    ThreadPool.QueueUserWorkItem(state => Checker(nextLink, accessToken));
                }
            }


   
            //Console.WriteLine("\n\nFinish");
        }

        public void Checker(string nextLink, string accessToken)
        {

            var entireJson = extractService.GetRequest(nextLink, accessToken);
            Console.WriteLine("  LINK  :  " + nextLink);

            if (entireJson["mime_type"] != null && entireJson["data"] != null)
            {
                Console.WriteLine(entireJson["mime_type"].ToString() + " : {\n" +  entireJson["data"].ToString() + "\n}");
            }

            //if (entireJson["mime_type"] != null)
            //{
            //    Console.WriteLine(entireJson["mime_type"]);
            //}
            if (entireJson["mime_type"] != null && entireJson["data"] == null)
            {
                Console.WriteLine("text/String : {\n" + entireJson["data"].ToString() + "\n}");
            }

            if (entireJson["link"] != null)
                GetRequest(entireJson);
            
        }

    }
}
