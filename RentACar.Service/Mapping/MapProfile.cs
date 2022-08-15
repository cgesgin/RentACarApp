using AutoMapper;
using RentACar.Core.DTOs;
using RentACar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Car, CarDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<CarDetails, CarDetailsDto>().ReverseMap();
            CreateMap<CarType, CarTypeDto>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<Costumer,CostumerDto >().ReverseMap();
            CreateMap<District,DistrictDto>().ReverseMap();
            CreateMap<Model,ModelDto>().ReverseMap();
            CreateMap<Payment,PaymentDto>().ReverseMap();
            CreateMap<Rental,RentalDto>().ReverseMap();
            CreateMap<RentalStore,RentalStoreDto>().ReverseMap();
        }
    }
}
