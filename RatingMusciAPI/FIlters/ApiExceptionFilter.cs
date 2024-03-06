using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RatingMusciAPI.FIlters;

public class ApiExceptionFilter:IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;
    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }
    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, context.Exception.Message);
        context.Result = new ObjectResult(new { Message = "An error occurred, please try again later: Status code 500" })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }   
}
