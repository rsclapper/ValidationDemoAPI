using ValidationDemoApi.Controllers;

namespace ValidationDemoApi.OrderService.ApiClients
{
    public interface IContactService
    {
        Task<List<ContactDto>?> GetContactsAsync();
    }
}