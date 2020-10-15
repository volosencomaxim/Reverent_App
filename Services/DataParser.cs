using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Reverent_App.Services
{
    class DataParser
    {
        public string CSVToJson(string dataSet)
        {
            var lines = dataSet.Split('\n');

            List<string[]> temp = new List<string[]>();
            foreach (var item in lines)
            {
                if (item == null || item == "\n" || item == "" || item == "\r")
                    continue;
                var chategories = item.Split(',');
                temp.Add(chategories);
            }

            string sTojson = "";

            for (int listIndex = 1; listIndex < temp.Count; listIndex++)
            {
                sTojson += "{";

                for (int stringIndex = 0; stringIndex < temp[0].Length; stringIndex++)
                {
                    sTojson += $"\"{temp[0][stringIndex]}\":\"{temp[listIndex][stringIndex]}\"";
                    if (stringIndex != temp[0].Length - 1)
                        sTojson += ",";

                }
                sTojson += "}";
                if (listIndex != temp.Count - 1)
                    sTojson += ",\n";
            }
            return sTojson;
        }

        public string XMLToJson(string dataSet)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(dataSet);

            string json = JsonConvert.SerializeXmlNode(doc);

            var match2 = Regex.Match(json, @"\[(.+?)\]", RegexOptions.Singleline).Groups[1].Value;
            return match2;
        }

        public string AdaptString(string dataSet)
        {
            var match = Regex.Match(dataSet, @"\[(.+?)\]", RegexOptions.Singleline).Groups[1].Value;

            return match;
        }

        public string XYAMToJson(string dataSet)
        {
            var xyam = dataSet.Split('\n');


            List<string[]> temp = new List<string[]>();
            foreach (var item in xyam)
            {
                if (item == null || item == "\n" || item == "" || item == "\r")
                    continue;
                var chategories = item.Split(',');
                temp.Add(chategories);
            }

            string sTojson = "";

            for (int listIndex = 1; listIndex < temp.Count; listIndex++)
            {

                for (int stringIndex = 0; stringIndex < temp[listIndex].Length; stringIndex++)
                {
                    if (temp[listIndex][stringIndex].Contains("-") && listIndex == 1)
                    {
                        sTojson += "{";
                    }
                    else if (temp[listIndex][stringIndex].Contains("-") && listIndex != 1)
                        sTojson += "},\n{";

                    string input = temp[listIndex][stringIndex].Replace("\"", "").Replace("\'", "").Replace("-", "").Replace(" ", "");

                    sTojson += $"\"{input}\"";
                    if (stringIndex != temp[listIndex].Length - 1)
                        sTojson += ":";
                }

                if (listIndex != temp.Count - 1)
                    sTojson += ",\n";

            }
            sTojson += "}";

            return sTojson;
        }
    }
}
