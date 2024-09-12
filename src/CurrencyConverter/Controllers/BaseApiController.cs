using System.Net;
using CurrencyConverter.Model.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Controllers;

public class BaseApiController : ControllerBase
{
    protected ObjectResult MapResponse(BaseResponse response)
    {
        if (response.Success)
        {
            return Ok(response);
        }

        var problemDetails = new ValidationProblemDetails
        {
            Type = "string",
            Title = "Error occurred",
            Status = response.StatusCode == HttpStatusCode.NotFound ? 404 : 400,
            Errors = new Dictionary<string, string[]> { { "Message", [response.Message] } },
            Instance = $"{HttpContext.Request.Method} {HttpContext.Request.Path}"
        };

        ObjectResult result = response.StatusCode switch
        {
            HttpStatusCode.NotFound => NotFound(problemDetails),
            HttpStatusCode.BadRequest => BadRequest(problemDetails),
            _ => BadRequest(problemDetails),
        };

        return result;
    }
}