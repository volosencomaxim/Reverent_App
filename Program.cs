using Reverent_App.Controllers;
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
            BurlacuController test = new BurlacuController();


            test.StartRouting();
            //test.HomeRoute();

            Console.ReadLine();
        }
    }
}
