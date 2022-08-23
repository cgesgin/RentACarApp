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
    public class DistrictsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IDistrictService _service;

        public DistrictsController(IMapper mapper, IDistrictService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var districts = await _service.GetAllAsync();
            var districtsDtos = _mapper.Map<List<DistrictDto>>(districts.ToList());
            return CreateActionResult(ResponseDto<List<DistrictDto>>.Success(200, districtsDtos));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var district = await _service.GetByIdAsync(id);
            var districtDto = _mapper.Map<DistrictDto>(district);
            return CreateActionResult(ResponseDto<DistrictDto>.Success(200, districtDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(DistrictDto districtDto)
        {
            var district = await _service.AddAsync(_mapper.Map<District>(districtDto));
            var districtDtos = _mapper.Map<DistrictDto>(district);
            return CreateActionResult(ResponseDto<DistrictDto>.Success(201, districtDtos));
        }

        [HttpPut]
        public async Task<IActionResult> Update(DistrictDto districtDto)
        {
            await _service.AnyAsync(x => x.Id == districtDto.Id);
            var district = _mapper.Map<District>(districtDto);
            await _service.UpdateAsync(district);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var district = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(district);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }
    }
}
