using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.Models
{
    public class Rental : BaseEntity
    {
        public DateTime? RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? Status { get; set; }
        public int? CostumerId { get; set; }
        public Costumer? Costumer { get; set; }
        public int? CarId { get; set; }
        public Car? Car { get; set; }
        public int? DropStoreId { get; set; }

        [ForeignKey("DropStoreId")]
        public RentalStore? DropStore { get; set; }
        public int? RentalAmount { get; set; }
        public int? TotalAmount { get; set; }
    }
}
