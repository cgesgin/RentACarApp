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
    public class BrandsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IBrandService _service;

        public BrandsController(IMapper mapper, IBrandService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _service.GetAllAsync();
            var brandsDtos = _mapper.Map<List<BrandDto>>(brands.ToList());
            return CreateActionResult(ResponseDto<List<BrandDto>>.Success(200, brandsDtos));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _service.GetByIdAsync(id);
            var brandDto = _mapper.Map<BrandDto>(brand);
            return CreateActionResult(ResponseDto<BrandDto>.Success(200, brandDto));
        }
        [HttpPost]
        public async Task<IActionResult> Save(BrandDto brandDto)
        {
            var brand = await _service.AddAsync(_mapper.Map<Brand>(brandDto));
            var brandsDto = _mapper.Map<BrandDto>(brand);
            return CreateActionResult(ResponseDto<BrandDto>.Success(201, brandsDto));
        }
        [HttpPut]
        public async Task<IActionResult> Update(BrandDto brandDto)
        {
            await _service.UpdateAsync(_mapper.Map<Brand>(brandDto));
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var brand = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(brand);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetByIdBrandWithModels(int id)
        {
            return CreateActionResult(await _service.GetByIdBrandWithModelsAsync(id));
        }
    }
}
