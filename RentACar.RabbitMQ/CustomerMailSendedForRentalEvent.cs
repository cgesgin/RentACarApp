using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.RabbitMQ
{
    public class CustomerMailSendedForRentalEvent
    {
        public DateTime ?ReturnDate { get; set; }
        public DateTime ?RentalDate { get; set; }
        public String CostumerName { get; set; }
        public String CostumerLastName { get; set; }
        public String CostumerEmail { get; set; }
        public String CarModel { get; set; }
        public String CarBrand { get; set; }
        public int? Amount  { get; set; }
        public string ?DropStore  { get; set; }
        public string ?RentalStores  { get; set; }
    }
}
