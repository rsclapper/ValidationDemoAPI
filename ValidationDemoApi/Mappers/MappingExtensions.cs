using ValidationDemoApi.Controllers;
using ValidationDemoApi.CORE.Models;

namespace ValidationDemoApi.Mappers
{
    public static class MappingExtensions
    {
        public static ContactDto MapToDto(this Contact source)
        {
            return new ContactDto
            {
                Id = source.Id,
                Name = source.Name,
                Email = source.Email,
                Phone = source.Phone,
                Address = source.Address,
                City = source.City,
                Region = source.Region,
                PostalCode = source.PostalCode,
                Birthday = source.Birthday,
                Salary = source.Salary
            };
        }
        public static Contact MapToEntity(this ContactDto source)
        {
            return new Contact
            {
                Id = source.Id,
                Name = source.Name,
                Email = source.Email,
                Phone = source.Phone,
                Address = source.Address,
                City = source.City,
                Region = source.Region,
                PostalCode = source.PostalCode,
                Birthday = source.Birthday,
                Salary = source.Salary
            };
        }

        public static OrderDto MapToDto(this Order source)
        {
            return new OrderDto
            {
                Id = source.Id,
                ContactId = source.ContactId,
                OrderDate = source.OrderDate,
                Total = source.Total
            };
        }

        public static Order MapToEntity(this OrderDto source)
        {
            return new Order
            {
                Id = source.Id,
                ContactId = source.ContactId,
                OrderDate = source.OrderDate,
                Total = source.Total
            };
        }
    }
}
