using System;
using System.Net;

namespace HttpHandlerExample
{
    public class PVGHttpHandler : IHttpHandler
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpListenerContext context)
        {
            // Проверяем метод запроса
            if (context.Request.HttpMethod == "GET")
            {
                // Получаем значения параметров ParmA и ParmB из запроса
                string parmA = context.Request.QueryString.Get("ParmA");
                string parmB = context.Request.QueryString.Get("ParmB");

                // Формируем текст для ответа
                string responseText = $"GET-Http-PVG:ParmA = {parmA},ParmB = {parmB}";

                // Устанавливаем MIME-тип ответа
                context.Response.ContentType = "text/plain";

                // Отправляем ответ
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseText);
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                // Если метод запроса не GET, отправляем ошибку "Метод не поддерживается"
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }

            // Завершаем ответ
            context.Response.OutputStream.Close();
        }
    }
}