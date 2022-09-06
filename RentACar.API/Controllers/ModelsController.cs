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
    public class ModelsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IModelService _service;

        public ModelsController(IMapper mapper, IModelService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carModels = await _service.GetAllAsync();
            var carModelsDtos = _mapper.Map<List<ModelDto>>(carModels.ToList());
            return CreateActionResult(ResponseDto<List<ModelDto>>.Success(200, carModelsDtos));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var carModel = await _service.GetByIdAsync(id);
            var carModelDto = _mapper.Map<ModelDto>(carModel);
            return CreateActionResult(ResponseDto<ModelDto>.Success(200, carModelDto));
        }
        [HttpPost]
        public async Task<IActionResult> Save(ModelDto carModelDto)
        {
            var carModel = await _service.AddAsync(_mapper.Map<Model>(carModelDto));
            var carModelsDto = _mapper.Map<ModelDto>(carModel);
            return CreateActionResult(ResponseDto<ModelDto>.Success(201, carModelsDto));
        }
        [HttpPut]
        public async Task<IActionResult> Update(ModelDto carModelDto)
        {
            var carModel = _mapper.Map<Model>(carModelDto);
            await _service.UpdateAsync(carModel);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var carModel = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(carModel);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }
        [ServiceFilter(typeof(NotFoundFilter<Model>))]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetModelWithBrand() 
        {
            return CreateActionResult(await _service.GetModelsWithBrandAsync());
        }
    }
}
