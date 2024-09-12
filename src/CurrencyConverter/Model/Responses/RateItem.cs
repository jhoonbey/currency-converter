namespace CurrencyConverter.Model.Responses;

public class RateItem
{
    public string Date { get; set; }
    public string Currency { get; set; }
    public decimal Rate { get; set; }
}