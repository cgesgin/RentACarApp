using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core
{
    public class District : BaseEntity
    {
        public string? Name { get; set; }
        public int CityId { get; set; }
        public City? City { get; set; }
    }
}
