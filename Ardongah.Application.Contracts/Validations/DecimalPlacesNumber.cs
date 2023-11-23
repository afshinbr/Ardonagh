using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ardonagh.Application.Contracts.Validations
{
    public class DecimalPlacesNumber : ValidationAttribute, IClientModelValidator
    {
        // Get the number of the maximum decimal from the data annotation
        private readonly int _maxDecimal;

        public DecimalPlacesNumber(int maxDecimal)
        {
            _maxDecimal = maxDecimal;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var number =  Convert.ToString(value);
            var decimalPlaces = 0;
            // check if the input number has any decimal and found the decimal length
            if (number.Contains("."))
                decimalPlaces = number.Split('.')[1].Length;
            // Check if the input decimal is greater that the valid decimal numbers or not
            if (decimalPlaces != 0 && decimalPlaces > _maxDecimal)
                return new ValidationResult($"Only {_maxDecimal} decimal places is allowed.");

            return ValidationResult.Success;
        }

        // this part is used to have real data validation in frontend using jquery.validate.unobtrusive 
        public void AddValidation(ClientModelValidationContext context)
        {
            // sending the required information to the front-end
            context.Attributes.Add("data-val-decimalPlacesNumber-decimal",_maxDecimal.ToString());
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-decimalPlacesNumber", ErrorMessage);
        }
    }
}
