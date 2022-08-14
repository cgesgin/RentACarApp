using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core
{
    public class Payment : BaseEntity
    {
        public int CardNo { get; set; }
        public string? CardName { get; set; }
        public int CVV { get; set; }
        public DateTime ExpriyDate { get; set; }
        public int Amount { get; set; }
        public int RentalId { get; set; }
        public Rental? Rentals { get; set; }
    }
}
