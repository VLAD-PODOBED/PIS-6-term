using System;
using System.Net;

namespace HttpHandlerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // ������ ����� ������������� � ����������
            string prefix = "http://localhost:8080/";
            string extension = "PVG";

            // ������� ������ HttpListener � ��������� �������
            using (var listener = new HttpListener())
            {
                listener.Prefixes.Add(prefix + extension + "/");

                // �������� �������������
                listener.Start();
                Console.WriteLine("������ �������...");

                // ������������ �������� �������
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
