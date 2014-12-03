using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

// offered to the public domain for any use with no restriction
// and also with no warranty of any kind, please enjoy. - David Jeske. 

// simple HTTP explanation
// http://www.jmarshall.com/easy/http/

namespace Bend.Util 
{
    public class TestMain
    {
        public static int Main(String[] args)
        {
            HttpServer httpServer;
            if (args.GetLength(0) > 0)
            {
                String[] address = args[0].ToString().Split(':');
                if(address.Length != 2)
                    return abort();
                String[] ipParts = address[0].ToString().Split('.');
                if (ipParts.Length != 4)
                    return abort();
                byte[] ip = {0, 0, 0, 0};
                for(int i = 0; i < ipParts.Length; i++)
                {
                    ip[i] = Convert.ToByte(ipParts[i]);
                }
                int port = Convert.ToInt16(address[1]);
                httpServer = new MyHttpServer(ip, port);
            }
            else
            {
                httpServer = new MyHttpServer(80);
            }
            httpServer.HTTPRoot = @".";
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
            Console.WriteLine("HTTPServer running on address {0}:{1}", httpServer.Address, httpServer.Port);
            return 0;
        }

        public static int abort()
        {
            Console.WriteLine("Aborting!");
            return 1;
        }
    }
}



