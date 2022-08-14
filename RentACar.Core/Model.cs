using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core
{
    public  class Model : BaseEntity
    {
        public string? Name { get; set; }
        public int BrandId { get; set; }
        public Brand? Brand { get; set; }

    }
}
