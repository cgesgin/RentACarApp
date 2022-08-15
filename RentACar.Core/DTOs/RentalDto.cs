using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.DTOs
{
    public class RentalDto : BaseDto
    {
        public DateTime? RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? Status { get; set; }
        public int? CostumerId { get; set; }
        public int? CarId { get; set; }
        public int? DropStoreId { get; set; }
        public int? RentalAmount { get; set; }
        public int? TotalAmount { get; set; }
    }
}
