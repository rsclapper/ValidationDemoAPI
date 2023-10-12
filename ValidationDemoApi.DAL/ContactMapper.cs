using ValidationDemoApi.CORE.Interfaces;
using ValidationDemoApi.CORE.Models;

namespace ValidationDemoApi.DAL
{
    public class ContactMapper : IMapper<Contact>
    {
        public string MapToCSV(Contact source)
        {
            return $"{source.Id},{source.Name},{source.Email},{source.Phone},{source.Address},{source.City},{source.Region},{source.PostalCode},{source.Salary}";
        }

        public Contact MapToObject(string source)
        {
            var fields = source.Split(',');
            return new Contact
            {
                Id = int.Parse(fields[0]),
                Name = fields[1],
                Email = fields[2],
                Phone = fields[3],
                Address = fields[4],
                City = fields[5],
                Region = fields[6],
                PostalCode = fields[7],
                Salary = decimal.Parse(fields[8])
            };

        }
    }
}
