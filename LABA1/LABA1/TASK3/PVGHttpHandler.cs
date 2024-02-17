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
            if (context.Request.HttpMethod == "PUT")
            {
                string parmA;
                string parmB;
                using (StreamReader reader = new StreamReader(context.Request.InputStream))
                {
                    string requestBody = reader.ReadToEnd();
                    parmA = GetValueFromBody(requestBody, "ParmA");
                    parmB = GetValueFromBody(requestBody, "ParmB");
                }

                string responseText = $"PUT-Http-PVG:ParmA = {parmA},ParmB = {parmB}";

                // Устанавливаем MIME-тип ответа
                context.Response.ContentType = "text/plain";

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseText);
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }

            context.Response.OutputStream.Close();
        }

        private string GetValueFromBody(string requestBody, string paramName)
        {
            string[] bodyParameters = requestBody.Split('&');
            foreach (string parameter in bodyParameters)
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

    public interface IHttpHandler
    {
    }
}
