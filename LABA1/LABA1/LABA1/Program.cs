using System;
using System.Net;

namespace HttpHandlerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/PVG/");

            listener.Start();

            Console.WriteLine("Сервер запущен...");

            while (listener.IsListening)
            {
                HttpListenerContext context = listener.GetContext();
                PVGHttpHandler handler = new PVGHttpHandler();
                handler.ProcessRequest(context);
            }

            listener.Stop();
        }
    }
}