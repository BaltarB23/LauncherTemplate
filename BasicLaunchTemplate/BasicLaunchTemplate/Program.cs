using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BasicLaunchTemplate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---Dummy project for simple login---");
            Console.WriteLine("Enter login-server-address ");
            string loginServerAddress = Console.ReadLine();
            try
            {
                IPAddress.Parse(loginServerAddress);

                LoginServerProvider loginServerProvider = new LoginServerProvider(loginServerAddress);

                //read in user credentials
                //loginServerProvider.Play(userName, password);
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
