using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentACar.Core.DTOs;
using RentACar.Core.Models;
using RentACar.Core.Services;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarDetailsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly ICarDetailsService _service;

        public CarDetailsController(IMapper mapper, ICarDetailsService service)
        {
            _mapper = mapper;
            _service = service;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carDetails = await _service.GetAllAsync();
            var carDetailsDtos = _mapper.Map<List<CarDetailsDto>>(carDetails.ToList());
            return CreateActionResult(ResponseDto<List<CarDetailsDto>>.Success(200, carDetailsDtos));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var carDetails = await _service.GetByIdAsync(id);
            var carDetailsDto = _mapper.Map<CarDetailsDto>(carDetails);
            return CreateActionResult(ResponseDto<CarDetailsDto>.Success(200, carDetailsDto));
        }
        
        [HttpPost]
        public async Task<IActionResult> Save(CarDetailsDto carDetailsDto)
        {
            var carDetails = await _service.AddAsync(_mapper.Map<CarDetails>(carDetailsDto));
            var carDetailsDtos = _mapper.Map<CarDetailsDto>(carDetails);
            return CreateActionResult(ResponseDto<CarDetailsDto>.Success(201, carDetailsDtos));
        }
        
        [HttpPut]
        public async Task<IActionResult> Update(CarDetailsDto carDetailsDto)
        {
            var carDetails =_mapper.Map<CarDetails>(carDetailsDto);
            await _service.UpdateAsync(carDetails);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var carDetails = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(carDetails);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }
    }
}
