using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.DTOs
{
    public class RentalWithCostumerDto :RentalDto
    {
        public CostumerDto? Costumer { get; set; }
    }
}
