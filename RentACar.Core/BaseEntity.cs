using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core
{
    public abstract class BaseEntity
    {
        public int id { get; set; }
        public bool ? isDeleted { get; set; }

    }
}
