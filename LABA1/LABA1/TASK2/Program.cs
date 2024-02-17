using System;
using System.Net;

namespace HttpHandlerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Задаем адрес прослушивания и расширение
            string prefix = "http://localhost:8080/";
            string extension = "PVG";

            // Создаем объект HttpListener и добавляем префикс
            using (var listener = new HttpListener())
            {
                listener.Prefixes.Add(prefix + extension + "/");

                // Стартуем прослушивание
                listener.Start();
                Console.WriteLine("Сервер запущен...");

                // Обрабатываем входящие запросы
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
