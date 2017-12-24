using System.Collections.Generic;
using System.Linq;
using HomeCinema.Data.Repositories;
using HomeCinema.Entities;

namespace HomeCinema.Data.Extensions
{
    public static class RentalExtensions
    {
        public static IEnumerable<Rental> GetStockRentals(this IEntityBaseRepository<Rental> rentalsRepository,
            IEnumerable<Stock> stocks)
        {
            var stockIds = stocks.Select(s => s.ID).Distinct();
            var rentals = rentalsRepository.GetAll().Where(r => stockIds.Contains(r.StockId));

            return rentals;
        }
    }
}