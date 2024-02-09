using Serilog.Context;

namespace Newsletter.Api.Middlewares;

public class RequestLogContextMiddleware(RequestDelegate next)
{
    public Task InvokeAsync(HttpContext context)
    {
        var correlationId = Guid.NewGuid().ToString();
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            context.Items["CorrelationId"] = correlationId;
            return next(context);
        }
    }
}