﻿using System;
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

namespace Reverent_App.Controllers
{
    class BurlacuController
    {
        string staringURL;
        string accessToken;


        ExtractService extractService;
        JObject dataConrainer;


        public BurlacuController()
        {
            staringURL = Global.AppSetUp.RootURL + "/register";
            extractService = new ExtractService();
            
            dataConrainer = extractService.GetRequest(staringURL);
            accessToken = dataConrainer["access_token"].ToString();
        }
        public JObject HomeRoute()
        {

            string nextLink = Global.AppSetUp.RootURL + dataConrainer["link"].ToString();

            var firstJson = extractService.GetRequest(nextLink, accessToken);

            return firstJson;
            //Console.WriteLine(token);
            //Console.WriteLine(nextLink);

        }

        public void GetRequest()
        {

            //string secondURL = Global.AppSetUp.RootURL + "/route/1";
            //string newURL = Global.AppSetUp.RootURL + dataConrainer["link"].ToString();


            //string accessToken = dataConrainer["access_token"].ToString();

            //var entireJson = extractService.GetRequest(secondURL, accessToken);

            var entireJson = HomeRoute();

            //Console.WriteLine(entireJson);

            //if (entireJson["data"] != null)
            //{
            //    Console.WriteLine(entireJson["data"]);
            //}

            //if (entireJson["mime_type"] != null)
            //{
            //    Console.WriteLine(entireJson["mime_type"]);
            //}
            //if (entireJson["data"] != null && entireJson["mime_type"] == null)
            //{
            //    Console.WriteLine("Data is of string type!!");
            //}

            if (entireJson["link"] != null)
            {
                var linkJson = JObject.Parse(entireJson["link"].ToString());

                foreach (JProperty property in linkJson.Properties())
                {
                    Console.WriteLine(property.Value);
                }
            }


   
            Console.WriteLine("\n\nFinish");
        }

        
    }
}
