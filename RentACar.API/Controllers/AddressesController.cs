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
    public class AddressesController :CustomBaseController
    {
        
        private readonly IMapper _mapper;
        private readonly IAddressService _service;

        public AddressesController(IMapper mapper, IAddressService service)
        {
            _mapper = mapper;
            _service = service;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var addresses = await _service.GetAllAsync();
            var addressesDtos = _mapper.Map<List<AddressDto>>(addresses.ToList());
            return CreateActionResult(ResponseDto<List<AddressDto>>.Success(200, addressesDtos));
        }
        [ServiceFilter(typeof(NotFoundFilter<Address>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var address = await _service.GetByIdAsync(id);
            var addressDto = _mapper.Map<AddressDto>(address);
            return CreateActionResult(ResponseDto<AddressDto>.Success(200, addressDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(AddressDto addressDto)
        {
            var address = await _service.AddAsync(_mapper.Map<Address>(addressDto));
            var addressDtos = _mapper.Map<AddressDto>(address);
            return CreateActionResult(ResponseDto<AddressDto>.Success(201, addressDtos));
        }
       
        [HttpPut]
        public async Task<IActionResult> Update(AddressDto addressDto)
        {
            await _service.AnyAsync(x => x.Id == addressDto.Id);
            var address = _mapper.Map<Address>(addressDto);            
            await _service.UpdateAsync(address);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }
        
        [ServiceFilter(typeof(NotFoundFilter<Address>))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var address = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(address);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAddressWithDistrict()
        {
            var addresses = await _service.GetAddressWithDistrictAsync();
            var addressesDtos = _mapper.Map<List<AddressWithDistrictDto>>(addresses.ToList());
            return CreateActionResult(ResponseDto<List<AddressWithDistrictDto>>.Success(200, addressesDtos));
        }
    }
}
