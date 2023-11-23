using System.ComponentModel.DataAnnotations;
using Ardonagh.Application.Contracts.Validations;

namespace Ardonagh.Application.Contracts
{
    public class CreateCustomer
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(50, ErrorMessage = "Name can have maximum 50 characters.")]
        public string Name { get;  set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(0,110, ErrorMessage = "Age can be between 0 to 110.")]
        public int Age { get;  set; }

        // Regex is used to check if the data consists only number, letters and space.
        [Required(ErrorMessage = "Post Code is required.")]
        [RegularExpression(@"^[A-Za-z0-9 ]+$", ErrorMessage = "Only numbers and characters are allowed.")]
        public string PostCode { get;  set; }

        // Custom validation DecimalPlacesNumber is used to check the number of decimal places.
        [Required(ErrorMessage = "Height is required.")]
        [Range(0, 2.5, ErrorMessage = "Height can be between 0 to 2.5.")]
        [DecimalPlacesNumber(2, ErrorMessage = $"Only 2 decimal places is allowed.")]
        public double Height { get;  set; }
    }
}
