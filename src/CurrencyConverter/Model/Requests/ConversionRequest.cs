using System.ComponentModel.DataAnnotations;
using CurrencyConverter.Constants;

namespace CurrencyConverter.Model.Requests;

public class ConversionRequest
{
    [Required(ErrorMessage = ValidationMessages.AmountRequired)]
    [RegularExpression(@"^-?\d+(\.\d+)?$", ErrorMessage = ValidationMessages.AmountMustDecimal)]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = ValidationMessages.FromCurrencyValueRequired)]
    [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = ValidationMessages.FromCurrencyValueMustCurrency)]
    public string FromCurrency { get; set; }

    [Required(ErrorMessage = ValidationMessages.ToCurrencyValueRequired)]
    [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = ValidationMessages.ToCurrencyValueMustCurrency)]
    public string ToCurrency { get; set; }
}