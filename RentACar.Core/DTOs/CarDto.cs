using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace RentACar.Core.DTOs
{
    public class CarDto : BaseDto
    {
        public int? SeatCapacity { get; set; }
        public string? GearType { get; set; }
        public string? FuelType { get; set; }
        public string? Status { get; set; }
        public int? Price { get; set; }
        public int? CarDetailsId { get; set; }         
        public int? RentalStoreId { get; set; }         
        public int? CarTypeId { get; set; }         
        public int? ModelId { get; set; }         
        public string? ImageName { get; set; }
        [DisplayName("Upload File")]
        public IFormFile? ImageFile { get; set; }
    }
}
