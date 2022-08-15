using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.DTOs
{
    public class CarDetailsDto : BaseDto
    {
        public int CarYear { get; set; }
        public string? Color { get; set; }
    }
}
