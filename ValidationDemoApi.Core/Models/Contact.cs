using System.ComponentModel.DataAnnotations;
using ValidationDemoApi.CORE.Interfaces;

namespace ValidationDemoApi.CORE.Models
{


    public class Contact : IEntity, IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage="Name must be between 2 - 50 characters")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Range(0, 1000000)]
        [Required]
        public decimal Salary { get; set; }

        public int UserId { get; set; }

        public virtual List<Order> Orders { get; set; } = new List<Order>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // can be used to validate multiple properties at once
            // We will check if the Birthday is between Today and 200 years ago

            var results = new List<ValidationResult>();

            if (Birthday > DateTime.Now || Birthday < DateTime.Now.AddYears(-200))
            {
                results.Add(new ValidationResult("Birthday must be between today and 200 years ago", new[] { nameof(Birthday) }));

            }

            return results;
        }
    }

    public class Order : IEntity
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }

        public virtual Contact Contact { get; set; }
    }
}
