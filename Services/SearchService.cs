using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reverent_App.Services
{
    class SearchService
    {

        private string dataContainer;
        public SearchService()
        {
            dataContainer = "";
        }


        public string CommandController(JArray jsonObject, string command)
        {

            List<string> commandContainer = new List<string>(
                command.Split(new string[] { " " }, StringSplitOptions.None));
            if (commandContainer[0].Contains("SelectColumn"))
                return SelectColumn(jsonObject, commandContainer[1]);
            if (commandContainer[0].Contains("SelectColumnWithData"))
                return SelectColumnWithData(jsonObject, commandContainer[1]);

            commandContainer.Clear();
            commandContainer.Add("Wrong command, try again.");
            return commandContainer[0];
        }
        private string SelectColumn(JArray jsonObject, string str)
        {
            var smth = jsonObject.Where(x => x is JObject y && y.ContainsKey(str)).ToArray();

            foreach (var a in smth)
            {
                dataContainer += a[str].ToString() + " ";
            }
            return dataContainer;
        }

        private string SelectColumnWithData(JArray jsonObject, string str)
        {
            var smth = jsonObject.Where(x => x is JObject y && y.ContainsKey(str)).ToArray();

            foreach (var a in smth)
            {
                dataContainer += a.ToString() + " ";
            }
            return dataContainer;
        }
    }
}
