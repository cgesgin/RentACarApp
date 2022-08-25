using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentACar.API.Filters;
using RentACar.Core.DTOs;
using RentACar.Core.Models;
using RentACar.Core.Services;

namespace RentACar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly ICarService _service;

        public CarsController(IMapper mapper, ICarService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cars = await _service.GetAllAsync();
            var carDtos = _mapper.Map<List<CarDto>>(cars.ToList());
            return CreateActionResult(ResponseDto<List<CarDto>>.Success(200, carDtos));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var car = await _service.GetByIdAsync(id);
            var carDto = _mapper.Map<CarDto>(car);
            return CreateActionResult(ResponseDto<CarDto>.Success(200, carDto));
        }
        [HttpPost]
        public async Task<IActionResult> Save(CarDto carDto)
        {
            var car = await _service.AddAsync(_mapper.Map<Car>(carDto));
            var carDtos = _mapper.Map<CarDto>(car);
            return CreateActionResult(ResponseDto<CarDto>.Success(201, carDtos));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CarDto carDto)
        {
            await _service.AnyAsync(x => x.Id == carDto.Id);
            await _service.UpdateAsync(_mapper.Map<Car>(carDto));
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var car = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(car);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCarWithFeature()
        {
            var cars = await _service.GetCarWithFeatureAsync();
            var carsDtos = _mapper.Map<List<CarWithFeatureDto>>(cars.ToList());
            return CreateActionResult(ResponseDto<List<CarWithFeatureDto>>.Success(200, carsDtos));
        }
        [ServiceFilter(typeof(NotFoundFilter<Car>))]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetByIdCarWithFeature(int id)
        {
            var cars = await _service.GetByIdCarWithFeatureAsync(id);
            var carsDtos = _mapper.Map<CarWithFeatureDto>(cars);
            return CreateActionResult(ResponseDto<CarWithFeatureDto>.Success(200, carsDtos));
        }
    }
}
