using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core
{
    public class CarDetails : BaseEntity
    {
        public int CarYear { get; set; }
        public string? Color { get; set; }
    }
}
