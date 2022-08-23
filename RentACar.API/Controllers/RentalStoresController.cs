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
    public class RentalStoresController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IRentalStoreService _service;

        public RentalStoresController(IMapper mapper, IRentalStoreService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rentalStores = await _service.GetAllAsync();
            var rentalStoreDtos = _mapper.Map<List<RentalStoreDto>>(rentalStores.ToList());
            return CreateActionResult(ResponseDto<List<RentalStoreDto>>.Success(200, rentalStoreDtos));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rentalStore = await _service.GetByIdAsync(id);
            var rentalStoreDto = _mapper.Map<RentalStoreDto>(rentalStore);
            return CreateActionResult(ResponseDto<RentalStoreDto>.Success(200, rentalStoreDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(RentalStoreDto rentalStoreDto)
        {
            var rentalStore = await _service.AddAsync(_mapper.Map<RentalStore>(rentalStoreDto));
            var resultRentalStore = _mapper.Map<RentalStoreDto>(rentalStore);
            return CreateActionResult(ResponseDto<RentalStoreDto>.Success(201, resultRentalStore));
        }

        [HttpPut]
        public async Task<IActionResult> Update(RentalStoreDto rentalStoreDto)
        {
            await _service.AnyAsync(x => x.Id == rentalStoreDto.Id);
            await _service.UpdateAsync(_mapper.Map<RentalStore>(rentalStoreDto));
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var rentalStore = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(rentalStore);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }
    }
}
