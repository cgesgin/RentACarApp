using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.Models
{
    public class Car : BaseEntity
    {
        public int? SeatCapacity { get; set; }
        public string? GearType { get; set; }
        public string? FuelType { get; set; }
        public string? Status { get; set; }
        public int? Price { get; set; }
        public int? CarDetailsId { get; set; }
        public CarDetails? CarDetails { get; set; }
        public int? RentalStoreId { get; set; }
        public RentalStore? RentalStore { get; set; }
        public int? CarTypeId { get; set; }
        public CarType? CarType { get; set; }
        public int? ModelId { get; set; }
        public Model? Model { get; set; }
        public string? ImageName { get; set; }
    }
}
