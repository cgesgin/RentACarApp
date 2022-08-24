using RentACar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.DTOs
{
    public class RentalStoreWithAddressDto : RentalStoreDto
    {
        public AddressWithDistrictDto ? Address { get; set; }
    }
}
