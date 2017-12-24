using System.Linq;
using HomeCinema.Data.Repositories;
using HomeCinema.Entities;

namespace HomeCinema.Data.Extensions
{
    public static class CustomerExtensions
    {
        public static bool UserExists(this IEntityBaseRepository<Customer> customersRepository, string email,
            string identityCard)
        {
            var userExists = false;

            userExists = customersRepository.GetAll()
                .Any(c => c.Email.ToLower() == email ||
                          c.IdentityCard.ToLower() == identityCard);

            return userExists;
        }

        public static string GetCustomerFullName(this IEntityBaseRepository<Customer> customersRepository,
            int customerId)
        {

            var customer = customersRepository.GetSingle(customerId);
            var customerName = customer.FirstName + " " + customer.LastName;

            return customerName;
        }
    }
}