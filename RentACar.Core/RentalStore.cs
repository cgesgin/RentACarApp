using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core
{
    public class RentalStore : BaseEntity
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? phone { get; set; }
        public int AddressId { get; set; }
        public Address? Address { get; set; }
        public List<Car>? Cars { get; set; }
    }
}
