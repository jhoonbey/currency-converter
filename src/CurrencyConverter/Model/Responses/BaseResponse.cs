using System.Net;
using System.Text.Json.Serialization;

namespace CurrencyConverter.Model.Responses;

public class BaseResponse
{
    private bool _success;
    private HttpStatusCode _statusCode;

    [JsonIgnore]
    public bool Success
    {
        get => _success;
        set
        {
            _success = value;
            if (value)
            {
                StatusCode = HttpStatusCode.OK;
            }
        }
    }

    [JsonIgnore]
    public HttpStatusCode StatusCode
    {
        get => _statusCode;
        set
        {
            _statusCode = value;
            if (_statusCode is not HttpStatusCode.OK)
            {
                Success = false;
            }
        }
    }

    [JsonIgnore] public string? Message { get; set; }

    public void SetResult(string message, HttpStatusCode statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }
}