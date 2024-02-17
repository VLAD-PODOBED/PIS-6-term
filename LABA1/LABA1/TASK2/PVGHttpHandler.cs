using System;
using System.IO;
using System.Net;

namespace HttpHandlerExample
{
    public class PVGHttpHandler : IHttpHandler
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpListenerContext context)
        {
            // Проверяем метод запроса
            if (context.Request.HttpMethod == "POST")
            {
                // Получаем значения параметров ParmA и ParmB из тела запроса
                string parmA;
                string parmB;
                using (StreamReader reader = new StreamReader(context.Request.InputStream))
                {
                    string requestBody = reader.ReadToEnd();
                    parmA = GetValueFromQueryString(requestBody, "ParmA");
                    parmB = GetValueFromQueryString(requestBody, "ParmB");
                }

                // Формируем текст для ответа
                string responseText = $"POST-Http-PVG:ParmA = {parmA},ParmB = {parmB}";

                // Устанавливаем MIME-тип ответа
                context.Response.ContentType = "text/plain";

                // Отправляем ответ
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseText);
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                // Если метод запроса не POST, отправляем ошибку "Метод не поддерживается"
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }

            // Завершаем ответ
            context.Response.OutputStream.Close();
        }

        private string GetValueFromQueryString(string queryString, string paramName)
        {
            string decodedQueryString = Uri.UnescapeDataString(queryString);
            string[] queryParameters = decodedQueryString.Split('&');
            foreach (string parameter in queryParameters)
            {
                string[] keyValue = parameter.Split('=');
                if (keyValue.Length == 2 && keyValue[0].Trim().ToLower() == paramName.ToLower())
                {
                    return Uri.UnescapeDataString(keyValue[1]);
                }
            }
            return null;
        }
    }
}
