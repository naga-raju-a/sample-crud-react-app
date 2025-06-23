using System.ComponentModel.DataAnnotations;

namespace Sample.Web.API.Models
{
    public class Cafe
    {
        // Unique identifier for the cafe, represented as a UUID (Guid in C#)
        [Required]
        public Guid Id { get; set; }

     
        [Required]
        [StringLength(100, MinimumLength = 1)]  
        public string Name { get; set; }

     
        [Required]
        [StringLength(500)] 
        public string Description { get; set; }

       
        public string Logo { get; set; }

      
        [Required]
        [StringLength(200)]  // Limiting location length to 200 characters
        public string Location { get; set; }
    }
}
