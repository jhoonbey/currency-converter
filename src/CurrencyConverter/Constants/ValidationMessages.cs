namespace CurrencyConverter.Constants;

public static class ValidationMessages
{
    public const string AmountRequired = "Amount is required";
    public const string FromCurrencyValueRequired = "FromCurrency is required";
    public const string ToCurrencyValueRequired = "ToCurrency is required";
    public const string StartDateRequired = "StartDate is required";
    public const string EndDateRequired = "EndDate is required";
    public const string AmountMustDecimal = "Amount must be a decimal";
    public const string FromCurrencyValueMustCurrency = "FromCurrency must be a currency";
    public const string ToCurrencyValueMustCurrency = "ToCurrency must be a currency";
    public const string StartDateMustDateFormat = "StartDate must be in YYYY-MM-DD format";
    public const string EndDateMustDateFormat = "EndDate must be in YYYY-MM-DD format";
    public const string PageNoMustBePositiveNumber = "PageNo must be a positive number";
    public const string PageSizeMustBePositiveNumber = "PageSize must be a positive number";
    public const string DontUseCurrencyError = "Do not use these currencies in conversion";
    public const string ApiCallError = "Error on calling the Frankfurter API";
}