using Reverent_App.Controllers;
using Reverent_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reverent_App
{
    class Program
    {
        static void Main(string[] args)
        {

            var rout = new RoutingController();
            Console.WriteLine("Wait about 10 seconds :D");
            rout.StartRouting();
        }
    }
}
