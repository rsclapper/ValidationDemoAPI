using ValidationDemoApi.Controllers;

namespace ValidationDemoApi.OrderService.ApiClients
{
    public interface IClientService
    {
        Task<List<ContactDto>?> GetContactsAsync();
    }
}