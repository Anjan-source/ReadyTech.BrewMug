namespace ReadyTech.BrewMug.Api.Middlewares
{
    public class CoffeeAvailabilityCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private int _requestCount = 0;

        public CoffeeAvailabilityCheckMiddleware(RequestDelegate next)
        { 
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _requestCount++;

            if (DateTime.Today.Month == 4 && DateTime.Today.Day == 1)
            {
                context.Response.StatusCode = 418;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("I'm a teapot.!");
                return;
            }
            else if (_requestCount == 5)
            {
                    context.Response.StatusCode = 503;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("Service Unavailable.!");

                    //reset to 0
                    _requestCount = 0;
                    return;
             
            }

            await _next(context);
        }
    }
}
