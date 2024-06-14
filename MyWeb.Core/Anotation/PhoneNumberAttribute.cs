using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace MyApp.Web.Anotation
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var phoneNumber = value as string;
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return new ValidationResult("Phone number is required.");
            }

            var regex = new Regex(@"^\+?[0-9]\d{1,14}$");
            if (!regex.IsMatch(phoneNumber))
            {
                return new ValidationResult("Invalid phone number format.");
            }

            return ValidationResult.Success;
        }
    }
}
