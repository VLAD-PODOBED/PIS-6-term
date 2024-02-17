using System;
using System.Net;

namespace HttpHandlerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var listener = new HttpListener())
            {
                listener.Prefixes.Add("http://localhost:8080/PVG/");

                listener.Start();

                Console.WriteLine("Сервер запущен...");

                while (true)
                {
                    var context = listener.GetContext();
                    var handler = new PVGHttpHandler();
                    handler.ProcessRequest(context);
                }
            }
        }
    }
}
