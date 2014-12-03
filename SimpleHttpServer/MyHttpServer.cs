using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Bend.Util
{
    public class MyHttpServer : HttpServer
    {
        public MyHttpServer(int port)
            : base(port)
        {
        }

        public MyHttpServer(byte[] address, int port)
            : base(address, port)
        {
        }

        public override void handleGETRequest(HttpProcessor p)
        {
            String url = p.http_url.ToLower();
            if (url.Contains(".png"))
            {
                Stream fs = File.Open(HTTPRoot + url, FileMode.Open);

                p.writeSuccess("image/png");
                fs.CopyTo(p.outputStream.BaseStream);
                p.outputStream.BaseStream.Flush();
                fs.Close();
            }

            if (url.Contains(".jpg"))
            {
                Stream fs = File.Open(HTTPRoot + url, FileMode.Open);

                p.writeSuccess("image/jpg");
                fs.CopyTo(p.outputStream.BaseStream);
                p.outputStream.BaseStream.Flush();
                fs.Close();
            }

            if (url.Contains(".pdb") || url.Contains(".exe") || url.Contains(".dll"))
            {
                Stream fs = File.Open(HTTPRoot + url, FileMode.Open);

                p.writeSuccess("application/octet-stream");
                fs.CopyTo(p.outputStream.BaseStream);
                p.outputStream.BaseStream.Flush();
                fs.Close();
            }

            Console.WriteLine("request: {0}", url);

            String page = getPage(url);
            if (page != "")
            {
                p.writeSuccess();
                p.outputStream.Write(page);
            }
            else
            {
                // send test page
                p.writeFailure();
            }
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            Console.WriteLine("POST request: {0}", p.http_url);
            string data = inputData.ReadToEnd();

            p.writeSuccess();
            p.outputStream.WriteLine("<html><body><h1>test server</h1>");
            p.outputStream.WriteLine("<a href=/test>return</a><p>");
            p.outputStream.WriteLine("postbody: <pre>{0}</pre>", data);
        }

        private String getPage(String pageName)
        {
            pageName = pageName.Replace("/", "");
            if (pageName == "")
                return "";
            String[] files = Directory.GetFiles(m_httpRoot);
            String page = "";
            foreach (String file in files)
            {
                if (file.Contains(pageName))
                {
                    StreamReader reader = new StreamReader(file);
                    page = reader.ReadToEnd();
                }
            }
            return page;
        }
    }
}