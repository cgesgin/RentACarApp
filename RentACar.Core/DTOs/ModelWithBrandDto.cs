using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.DTOs
{
    public class ModelWithBrandDto : ModelDto
    {
        public BrandDto Brand { get; set; }
    }
}
