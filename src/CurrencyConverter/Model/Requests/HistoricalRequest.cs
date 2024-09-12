using System.ComponentModel.DataAnnotations;
using CurrencyConverter.Constants;

namespace CurrencyConverter.Model.Requests;

public class HistoricalRequest
{
    [Required(ErrorMessage = ValidationMessages.StartDateRequired)]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = ValidationMessages.StartDateMustDateFormat)]
    public string StartDate { get; set; }

    [Required(ErrorMessage = ValidationMessages.EndDateRequired)]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = ValidationMessages.EndDateMustDateFormat)]
    public string EndDate { get; set; }

    public string? BaseCurrency { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.PageNoMustBePositiveNumber)]
    [RegularExpression("^[0-9]+$", ErrorMessage = ValidationMessages.PageNoMustBePositiveNumber)]
    public int PageNo { get; set; } = 1;

    [Range(1, 100, ErrorMessage = ValidationMessages.PageSizeMustBePositiveNumber)]
    [RegularExpression("^[0-9]+$", ErrorMessage = ValidationMessages.PageSizeMustBePositiveNumber)]
    public int PageSize { get; set; } = ApplicationConstants.DefaultPageSize;
}