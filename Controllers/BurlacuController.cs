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
        private string _rootURL;
        private string _accessToken;
        private string _homePath;

        private ExtractService _extractService;

        public BurlacuController()
        {
            _rootURL = Global.AppSetUp.RootURL;
            _extractService = new ExtractService();

            GetAccesToken();
        }


        public void StartRouting()
        {
            string homeLink = _rootURL + _homePath;

            var firstJson = _extractService.GetRequest(homeLink, _accessToken);

            LinkRouting(firstJson);

        }

        public void LinkRouting(JObject entireJson)
        {


            if (entireJson["link"] != null)
            {
                var linkJson = JObject.Parse(entireJson["link"].ToString());


                foreach (JProperty property in linkJson.Properties())
                {
                    string routeLink = _rootURL + property.Value;

                    ThreadPool.QueueUserWorkItem(state => Checker(routeLink, _accessToken));
                }
            }
        }

        public void Checker(string nextLink, string accessToken)
        {

            var entireJson = _extractService.GetRequest(nextLink, accessToken);
            Console.WriteLine("  LINK  :  " + nextLink);

            if (entireJson["mime_type"] != null && entireJson["data"] != null)
            {
                Console.WriteLine(entireJson["mime_type"].ToString() + " : {\n" +  entireJson["data"].ToString() + "\n}");
            }

            if (entireJson["mime_type"] == null && entireJson["data"] != null)
            {
                Console.WriteLine("text/string : {\n" + entireJson["data"].ToString() + "\n}");
            }

            if (entireJson["link"] != null)
                LinkRouting(entireJson);
            
        }

        private void GetAccesToken()
        {
            string _registerURL;
            string _registerPath;
            JObject _dataContainer;

            _dataContainer = _extractService.GetRequest(_rootURL);
            _registerPath = _dataContainer["register"]["link"].ToString();  

            _registerURL = _rootURL + _registerPath;
            _dataContainer = _extractService.GetRequest(_registerURL);

            _accessToken = _dataContainer["access_token"].ToString();
            _homePath = _dataContainer["link"].ToString();
        }

    }
}
