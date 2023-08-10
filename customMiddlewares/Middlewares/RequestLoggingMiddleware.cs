namespace customMiddlewares.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {

            /*
             * Request geldiğinde bilgi vermek
             * Response oluştuğunda yine bilgi vermek
             */
            _logger.LogInformation($"Gelen request metodu:{context.Request.Method}, adresi: {context.Request.Path}\t{DateTime.Now}");
            var responseBodyStream = context.Response.Body;
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            await _next(context);

            responseStream.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(responseStream).ReadToEnd();
            _logger.LogInformation($"Response tamamlandı...\n Tarih: {DateTime.Now}");
            _logger.LogInformation($"Response: {context.Response.StatusCode}");


            responseStream.Seek(0, SeekOrigin.Begin);
            await responseStream.CopyToAsync(responseBodyStream);


        }
    }
}
