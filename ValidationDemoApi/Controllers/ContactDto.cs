using System.ComponentModel.DataAnnotations;

namespace ValidationDemoApi.Controllers
{
    public class ContactDto 
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Email is not valid")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public DateTime Birthday { get; set; }
        public decimal Salary { get; set; }
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
    }
}