using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core
{
    public class Address : BaseEntity
    {
        public string? Street { get; set; }
        public string? Neighbourhood { get; set; }
        public string? description { get; set; }
        public int Zipcode { get; set; }
        public int DistrictId { get; set; }
        public District? District { get; set; }
    }
}
