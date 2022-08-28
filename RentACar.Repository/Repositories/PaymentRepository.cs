using Microsoft.EntityFrameworkCore;
using RentACar.Core.Models;
using RentACar.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Repository.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public Task<Rental> GetByIdPaymentWithRentalAsync(int rentalId)
        {
            var data =_appDbContext.Rentals
                .Include(x=>x.DropStore)
                .Include(x => x.Costumer)
                .Include(x=>x.Car)
                .ThenInclude(x => x.Model)
                .ThenInclude(x => x.Brand)
                .SingleOrDefault(x=>x.Id== rentalId);
            Car car = _appDbContext.Cars.Find(data.Car.Id);
            data.Car.RentalStore = car.RentalStore;           
            return Task.FromResult(data);
        }
    }
}
