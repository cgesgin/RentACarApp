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
    public class RentalsController : CustomBaseController
    {

        private readonly IMapper _mapper;
        private readonly IRentalService _service;

        public RentalsController(IMapper mapper, IRentalService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rentals = await _service.GetAllAsync();
            var rentalDtos = _mapper.Map<List<RentalDto>>(rentals.ToList());
            return CreateActionResult(ResponseDto<List<RentalDto>>.Success(200, rentalDtos));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rental = await _service.GetByIdAsync(id);
            var rentalDto = _mapper.Map<RentalDto>(rental);
            return CreateActionResult(ResponseDto<RentalDto>.Success(200, rentalDto));
        }
        [HttpPost]
        public async Task<IActionResult> Save(RentalDto rentalDto)
        {
            var rental = await _service.AddAsync(_mapper.Map<Rental>(rentalDto));
            var rentalDtos = _mapper.Map<RentalDto>(rental);
            return CreateActionResult(ResponseDto<RentalDto>.Success(201, rentalDtos));
        }
        [HttpPut]
        public async Task<IActionResult> Update(RentalDto rentalDto)
        {
            await _service.AnyAsync(x => x.Id == rentalDto.Id);
            var rental = _mapper.Map<Rental>(rentalDto);
            await _service.UpdateAsync(rental);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var rental = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(rental);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetByUserIdWithCarAndCostumer(string userId)
        {
            var rentals = await _service.GetByUserIdWithCarAndCostumerAsync(userId);
            var rentalDtos = _mapper.Map<List<RentalWithCarAndCostumerDto>>(rentals.ToList());
            return CreateActionResult(ResponseDto<List<RentalWithCarAndCostumerDto>>.Success(200, rentalDtos));
        }
    }
}
