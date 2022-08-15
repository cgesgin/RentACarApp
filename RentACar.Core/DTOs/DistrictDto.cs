using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.DTOs
{
    public class DistrictDto : BaseDto
    {
        public string? Name { get; set; }
        public int CityId { get; set; }
    }
}
