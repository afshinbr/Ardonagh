using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ardonagh.Application.Contracts.Validations
{
    public class DecimalPlacesNumber : ValidationAttribute, IClientModelValidator
    {
        private readonly int _maxDecimal;

        public DecimalPlacesNumber(int maxDecimal)
        {
            _maxDecimal = maxDecimal;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var number =  Convert.ToString(value);
            var decimalPlaces = 0;
            if (number.Contains("."))
                decimalPlaces = number.Split('.')[1].Length;
            if (decimalPlaces != 0 && decimalPlaces > _maxDecimal)
                return new ValidationResult($"Only {_maxDecimal} decimal places is allowed.");

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val-decimalPlacesNumber-decimal",_maxDecimal.ToString());
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-decimalPlacesNumber", ErrorMessage);
        }
    }
}
