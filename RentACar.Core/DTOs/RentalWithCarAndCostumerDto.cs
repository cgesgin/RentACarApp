using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.DTOs
{
    public class RentalWithCarAndCostumerDto :RentalDto
    {
        public CostumerDto ?Costumer { get; set; }
        public CarWithFeatureDto ?Car { get; set; }
        public RentalStoreDto? DropStore { get; set; }
    }
}
