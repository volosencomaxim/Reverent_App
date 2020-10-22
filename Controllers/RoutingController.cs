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
using System.Diagnostics;

namespace Reverent_App.Controllers
{
    class RoutingController
    {
        private string _rootURL;
        private string _accessToken;
        private string _homePath;

        private ExtractService _extractService;
        private TCPServer _connection;


        public RoutingController()
        {
            _rootURL = Global.AppSetUp.RootURL;
            _extractService = new ExtractService();
            _connection = new TCPServer();

            GetAccesToken();
        }


        public void StartRouting()
        {
            string homeLink = _rootURL + _homePath;

            var firstJson = _extractService.GetRequest(homeLink, _accessToken);

            LinkRouting(firstJson);

        }

        private void GetAccesToken()
        {
            JObject _dataContainer = _extractService.GetRequest(_rootURL);
            string _registerPath = _dataContainer["register"]["link"].ToString();

            string _registerURL = _rootURL + _registerPath;
            _dataContainer = _extractService.GetRequest(_registerURL);

            _accessToken = _dataContainer["access_token"].ToString();
            _homePath = _dataContainer["link"].ToString();
        }


        int index = 8;
        private void LinkRouting(JObject entireJson)
        {
            var linkJson = JObject.Parse(entireJson["link"].ToString());

            using (ManualResetEvent resetEvent = new ManualResetEvent(false))
            {

                foreach (JProperty property in linkJson.Properties())
                {
                    string routeLink = _rootURL + property.Value;

                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        DataExtractor(routeLink, _accessToken);

                        if (Interlocked.Decrement(ref index) == 0)
                            resetEvent.Set();

                    });
                }
                resetEvent.WaitOne();
            }

            var structuredData = _extractService.RefactoredJsonData();
            _connection.StartServer(structuredData);
        }



    private void DataExtractor(string nextLink, string accessToken)
        {

            var entireJson = _extractService.GetRequest(nextLink, accessToken);

            if (entireJson["data"] != null)
            {
                if (entireJson["mime_type"] != null)
                    _extractService.DataFilter(entireJson["mime_type"].ToString(), entireJson["data"].ToString());                
                else
                    _extractService.DataFilter("text/string", entireJson["data"].ToString());
            }

            if (entireJson["link"] != null)
                LinkRouting(entireJson);            
        }
    }
}
