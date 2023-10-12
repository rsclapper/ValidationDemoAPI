using ValidationDemoApi.Controllers;
using ValidationDemoApi.CORE.Models;

namespace ValidationDemoApi.OrderService.Mappers
{
    public static class MappingExtensions
    {
       
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
