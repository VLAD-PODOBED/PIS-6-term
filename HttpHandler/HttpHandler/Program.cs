using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
internal class Program
{
    private static string ResultString(string method, string paramFierst, string paramSecond)
    {
        if (method == null || paramFierst == null || paramSecond == null) return "Параметры не заданы корректно";
        return $"{method}-Http-PVG:ParmA = {paramFierst},ParmB = {paramSecond}";
    }
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/{id}.PVG", (string? ParmA, string? ParmB) => ResultString("GET", ParmA, ParmB));

        app.MapPost("/{id}.PVG", (string? ParmA, string? ParmB) => ResultString("POST", ParmA, ParmB));

        app.MapPut("/{id}.PVG", (string? ParmA, string? ParmB) => ResultString("PUT", ParmA, ParmB));

        app.MapPost("/PVGSum",  (HttpContext context) =>
        {
            try
            {
                var form = context.Request.ReadFormAsync();
                var xValue = form.Result["X"];
                var yValue = form.Result["Y"];

                var result = new ObjectResult(Convert.ToInt32(xValue) + Convert.ToInt32(yValue));
                string response = result.Value.ToString();
                return context.Response.WriteAsync(response);
            }
            catch (Exception ex)
            {
                return context.Response.WriteAsync(ex.Message);
            }
        });

        app.Map("/MultiplyingNumbers", (HttpContext context) =>
        {
            if (context.Request.Method == HttpMethods.Get)
            {
                return context.Response.WriteAsync(@"
                <html>
                    <body>
                        <script>
                            function send() {
                                var xhr = new XMLHttpRequest();
                                var x = 4;
                                var y = 8;
                                xhr.open('POST', `MultiplyingNumbers?x=${x}&y=${y}`, true);
                                xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                                xhr.onreadystatechange = function() {
                                    if (xhr.readyState !== XMLHttpRequest.DONE) return; 
                                    alert('Result: ' + xhr.responseText);
                                }
                                xhr.send();
                            }
                            send();
                        </script>;
                    </body>
                </html>");
            }
            else if (context.Request.Method == HttpMethods.Post)
            {
                if (int.TryParse(context.Request.Query["x"], out int x) && int.TryParse(context.Request.Query["y"], out int y))
                {
                    int product = x * y;
                    return context.Response.WriteAsync(product.ToString());
                }
                else
                {
                    return context.Response.WriteAsync("Invalid parameters. x and y must be integers.");
                }
            }
            else
            {
                context.Response.StatusCode = 405;
                return  context.Response.WriteAsync("Method not allowed");
            }
        });

        app.Map("MultiplyingNumbersForm", (context) =>
        {
            if (context.Request.Method == HttpMethods.Get)
            {
                context.Response.Headers.Add("Content-Type", "text/html");
                return context.Response.WriteAsync(@"
                <html>
                    <body>
                        <form action='MultiplyingNumbersForm' method='post'>
                            <input type='number' name='x' />
                            <input type='number' name='y' />
                            <input type='submit' value='Multiply' />
                        </form>
                    </body>
                </html>
                ");
            }
            else if (context.Request.Method == HttpMethods.Post)
            {

                if (int.TryParse(context.Request.Form["x"], out int x) && int.TryParse(context.Request.Form["y"], out int y))
                {
                    int product = x * y;
                    return context.Response.WriteAsync(product.ToString());
                }
                else
                {
                    context.Response.StatusCode = 400;
                    return context.Response.WriteAsync("Invalid parameters. x and y must be integers.");
                }
            }
            else
            {
                context.Response.StatusCode = 405;
                return context.Response.WriteAsync("Method not allowed");
            }
        });

        app.UseStaticFiles();

        app.Run();
    }
}