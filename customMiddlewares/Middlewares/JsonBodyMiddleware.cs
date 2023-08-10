using System.Text;

namespace customMiddlewares.Middlewares
{

    public class JsonBodyMiddleware
    {
        private readonly RequestDelegate _next;

        public JsonBodyMiddleware(RequestDelegate next)
        {
            _next = next;
        }



        public async Task Invoke(HttpContext context)
        {

            if (isAvailableForFilter(context))
            {
                var jsonRequestBody = await getJsonRequestBody(context);
                if (!string.IsNullOrEmpty(jsonRequestBody))
                {
                    replaceRequestBodyWithJsonStream(context, jsonRequestBody);
                    saveJsonToContextItem(context, jsonRequestBody);
                }

            }

            await _next(context);
        }

        private bool isAvailableForFilter(HttpContext context)
        {
            return (isPostRequest(context) || isPutRequest(context)) && isJsonRequest(context);
        }

        private async Task<string> getJsonRequestBody(HttpContext context)
        {
            using var reader = new StreamReader(context.Request.Body);
            return await reader.ReadToEndAsync();
        }

        private void replaceRequestBodyWithJsonStream(HttpContext context, string requestBody)
        {
            var content = Encoding.UTF8.GetBytes(requestBody);
            var requestBodyStream = new MemoryStream();
            requestBodyStream.Write(content, 0, content.Length);
            context.Request.Body = requestBodyStream;
            context.Request.Body.Seek(0, SeekOrigin.Begin);
        }

        private void saveJsonToContextItem(HttpContext context, string jsonRequestBody)
        {
            context.Items["jsonBody"] = jsonRequestBody;
        }

        class HttpMethods
        {
            public const string POST = "POST";
            public const string PUT = "PUT";
        }

        private bool isPostRequest(HttpContext context)
        {
            return context.Request.Method == HttpMethods.POST;
        }

        private bool isPutRequest(HttpContext context)
        {
            return context.Request.Method == HttpMethods.PUT;
        }
        private bool isJsonRequest(HttpContext context)
        {
            return context.Request.ContentType.StartsWith("application/json");
        }






    }
}
