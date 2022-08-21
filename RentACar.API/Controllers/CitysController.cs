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
    public class CitysController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly ICityService _service;

        public CitysController(IMapper mapper, ICityService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cities = await _service.GetAllAsync();
            var citiesDtos = _mapper.Map<List<CityDto>>(cities.ToList());
            return CreateActionResult(ResponseDto<List<CityDto>>.Success(200, citiesDtos));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var city = await _service.GetByIdAsync(id);
            var cityDto = _mapper.Map<CityDto>(city);
            return CreateActionResult(ResponseDto<CityDto>.Success(200, cityDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(CityDto cityDto)
        {
            var city = await _service.AddAsync(_mapper.Map<City>(cityDto));
            var cityDtos = _mapper.Map<CityDto>(city);
            return CreateActionResult(ResponseDto<CityDto>.Success(201, cityDtos));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CityDto cityDto)
        {
            var city = _mapper.Map<City>(cityDto);
            await _service.UpdateAsync(city);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var city = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(city);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }
    }
}