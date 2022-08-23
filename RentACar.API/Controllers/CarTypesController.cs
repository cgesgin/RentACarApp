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
    public class CarTypesController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly ICarTypeService _service;

        public CarTypesController(IMapper mapper, ICarTypeService service)
        {
            _mapper = mapper;
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carTypes = await _service.GetAllAsync();
            var carTypesDtos = _mapper.Map<List<CarTypeDto>>(carTypes.ToList());
            return CreateActionResult(ResponseDto<List<CarTypeDto>>.Success(200, carTypesDtos));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var carType = await _service.GetByIdAsync(id);
            var carTypeDto = _mapper.Map<CarTypeDto>(carType);
            return CreateActionResult(ResponseDto<CarTypeDto>.Success(200, carTypeDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(CarTypeDto addressDto)
        {
            var address = await _service.AddAsync(_mapper.Map<CarType>(addressDto));
            var addressDtos = _mapper.Map<CarTypeDto>(address);
            return CreateActionResult(ResponseDto<CarTypeDto>.Success(201, addressDtos));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CarTypeDto carTypeDto)
        {
            await _service.AnyAsync(x => x.Id == carTypeDto.Id);
            var carType = _mapper.Map<CarType>(carTypeDto);
            await _service.UpdateAsync(carType);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }

        [ServiceFilter(typeof(NotFoundFilter<CarType>))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var carType = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(carType);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }
    }
}
