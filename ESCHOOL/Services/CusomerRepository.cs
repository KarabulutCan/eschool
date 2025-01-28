using ESCHOOL.Models;
using System.Linq;

namespace ESCHOOL.Services
{
    public class CustomersRepository : ICustomersRepository
    {
        private CustomersDbContext _customersContext;
        public CustomersRepository(CustomersDbContext customersContext)
        {
            _customersContext = customersContext;
        }

        public Customers GetCustomer(int customerID)
        {
            return _customersContext.Customers.Where(b => b.CustomerID == customerID).FirstOrDefault();
        }



    }
}
