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
using System.Threading;

namespace Reverent_App.Controllers
{
    class RoutingController
    {
        private string _rootURL;
        private string _accessToken;
        private string _homePath;

        private ExtractService _extractService;

        public RoutingController()
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

        private void GetAccesToken()
        {
            JObject _dataContainer = _extractService.GetRequest(_rootURL);
            string _registerPath = _dataContainer["register"]["link"].ToString();

            string _registerURL = _rootURL + _registerPath;
            _dataContainer = _extractService.GetRequest(_registerURL);

            _accessToken = _dataContainer["access_token"].ToString();
            _homePath = _dataContainer["link"].ToString();
        }


        List<int> list = new List<int>();
        //int counter = -1;
        //int index = 0;

        private void LinkRouting(JObject entireJson)
        {
            var linkJson = JObject.Parse(entireJson["link"].ToString());

            int toProcess = 10;

            using (ManualResetEvent resetEvent = new ManualResetEvent(false))
            {
                //for (int i = index; i < linkJson.Count + index; i++)
                //{
                //    list.Add(i);
                //}
                //index += linkJson.Count;

                //foreach (var item in list)
                //{
                //    Console.WriteLine(item);
                //}
                for (int i = 0; i < linkJson.Count; i++) list.Add(i);

                int counter = -1;

                foreach (JProperty property in linkJson.Properties())
                {
                    counter += 1;
                    string routeLink = _rootURL + property.Value;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
                    {
                        DataExtractor(routeLink, _accessToken);
                        if (Interlocked.Decrement(ref toProcess) == 0)
                            resetEvent.Set();
                    }), list[counter]);
                }

                resetEvent.WaitOne();
            }
            ///

            //_extractService.ShowRezult();
            Console.WriteLine("\n\n\nDone\n\n\n");

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
