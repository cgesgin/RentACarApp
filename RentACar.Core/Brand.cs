using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core
{
    public class Brand : BaseEntity
    {
        public string? Name { get; set; }
        public List<Model>? Models { get; set; }
    }
}
