using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.DTOs
{
    public class BrandWithModelsDto : BrandDto
    {
        public List<ModelDto> Models { get; set; }
    }
}
