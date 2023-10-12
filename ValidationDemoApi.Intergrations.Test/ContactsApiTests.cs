using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using ValidationDemoApi.Controllers;
using ValidationDemoApi.CORE.Models;
using ValidationDemoApi.DAL;

namespace ValidationDemoApi.Intergrations.Test
{


    [TestFixture]
    public class ContactsApiTests
    {
        private CustomWebApplicationFactory<Program> _factory;
        private HttpClient _client;
        public ContactsApiTests()
        {

        }

        [SetUp]
        public void Setup()
        {
            _factory?.Dispose(); // Dispose the old factory if it exists
            _factory = new CustomWebApplicationFactory<Program>();
            // Initialize a client to ensure the TestServer is built.
            _client = _factory.CreateClient();
            // Create database
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ContactContext>();
                context.Database.EnsureCreated();
            }

            // ... Rest of the setup code...
        }
        
        [TearDown]

        public void Teardown()
        {
            // Delete database
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ContactContext>();
                context.Database.EnsureDeleted();
                context.Dispose();
            }
            _factory.Dispose();
        }

       

        private async Task<string> GetJwtTokenAsync(HttpClient client)
        {
            var loginData = new
            {
                username = "admin",
                password = "admin"
            };

            var response = await client.PostAsync("/api/auth/login", new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
            return tokenResponse.token; // Adjust this based on the structure of your token response
        }


        [Test]
        public async Task GetContacts_ReturnsExpectedContacts()
        {
            // Arrange
            

            // Act
            var response = await _client.GetAsync("/api/contacts");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var contacts = JsonConvert.DeserializeObject<IEnumerable<ContactDto>>(responseContent);
            // Perform more assertions with NUnit and FluentValidation here
            //Assert.AreEqual(0, contacts.Count());
        }

        [Test]
        public async Task AddContact_ReturnsSuccessResponse()
        {
            // Arrange
  
            var newContact = new ContactDto
            {
                Name = "John Smith",
                Email = "john.doe@example.com",
                Phone = "1234567890",
                Address = "123 Main St",
                City = "New York",
                Region = "NY",
                PostalCode = "12345",
                Salary = 100000
                // Set other properties as needed
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(newContact), Encoding.UTF8, "application/json");

            var token = await GetJwtTokenAsync(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.PostAsync("/api/contacts", jsonContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var returnedContact = JsonConvert.DeserializeObject<Contact>(responseContent);
            Assert.IsNotNull(returnedContact);
            Assert.AreEqual(newContact.Name, returnedContact.Name);
            Assert.AreEqual(newContact.Email, returnedContact.Email);
            // Perform more assertions with NUnit and FluentValidation here, if needed
        }

        [Test]
        public async Task AddContact_WithInvalidEmail_ReturnsValidationError()
        {
            // Arrange
            
            var newContact = new Contact
            {
                Name = "John Doe",
                Email = "invalid-email"
                // Set other properties as needed
            };
            var token = await GetJwtTokenAsync(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(JsonConvert.SerializeObject(newContact), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/contacts", jsonContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode); // Assuming your API returns a 400 Bad Request for validation errors

            var content = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(content.Contains("Email is not valid")); // Adjust this to match the actual error message your API returns
        }

    }

}