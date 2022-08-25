using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.DTOs
{
    public class CarWithFeatureDto : CarDto
    {
        public ModelWithBrandDto? Model { get; set; }
        public CarDetailsDto? CarDetails { get; set; }
        public CarTypeDto? CarType { get; set; }
        public RentalStoreDto? RentalStore { get; set; }
    }
}
