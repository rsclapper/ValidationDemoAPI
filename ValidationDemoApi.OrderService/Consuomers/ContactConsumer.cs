using System.ComponentModel.DataAnnotations;

namespace ValidationDemoApi.OrderService.Consuomers
{
    public class ContactAddConsumer
    {
    }

    public interface IContactAddConsumer
    {
        public string Name { get;  }
        public string Phone { get;  }
        public string Address { get;  }
        public string City { get;  }
        public string Region { get;  }
        public string PostalCode { get;  }
        public DateTime Birthday { get;  }
        public decimal Salary { get;  }


    }
}
