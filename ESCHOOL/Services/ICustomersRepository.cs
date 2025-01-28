using ESCHOOL.Models;

namespace ESCHOOL.Services
{
    public interface ICustomersRepository
    {
        Customers GetCustomer(int customerID);
    }
}
