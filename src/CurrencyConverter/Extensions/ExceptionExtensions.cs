namespace CurrencyConverter.Extensions;

public static class ExceptionExtensions
{
    public static Exception GetOriginalException(this Exception ex)
    {
        while (true)
        {
            if (ex.InnerException == null) return ex;
            ex = ex.InnerException;
        }
    }
}