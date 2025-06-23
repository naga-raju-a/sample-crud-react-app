using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sample.Web.API.Models
{
    public class Employee
    {
        public string? Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)] // Ensuring a non-empty name, max length of 100
        public string Name { get; set; }

      
        [Required]
        [EmailAddress] // Validates if the email address is in the correct format
        public string EmailAddress { get; set; }

       
        [Required]
        [RegularExpression(@"^[89]\d{7}$", ErrorMessage = "Phone number must start with 8 or 9 and contain 8 digits.")]
        public string PhoneNumber { get; set; }

       
        [Required]
        [RegularExpression(@"^(Male|Female)$", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public string Gender { get; set; }

        public string? CafeId { get; set; }

        public DateTime? EmploymentDate { get; set; }
    }

}
