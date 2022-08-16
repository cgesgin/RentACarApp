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
    public class CostumersController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IService<Costumer> _service;

        public CostumersController(IMapper mapper, IService<Costumer> service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var costumers = await _service.GetAllAsync();
            var costumersDtos = _mapper.Map<List<CostumerDto>>(costumers.ToList());
            return CreateActionResult(ResponseDto<List<CostumerDto>>.Success(200, costumersDtos));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var costumer = await _service.GetByIdAsync(id);
            var costumerDto = _mapper.Map<CostumerDto>(costumer);
            return CreateActionResult(ResponseDto<CostumerDto>.Success(200, costumerDto));
        }
        [HttpPost]
        public async Task<IActionResult> Save(CostumerDto costumerDto)
        {
            var costumer = await _service.AddAsync(_mapper.Map<Costumer>(costumerDto));
            var costumersDto = _mapper.Map<CostumerDto>(costumer);
            return CreateActionResult(ResponseDto<CostumerDto>.Success(201, costumersDto));
        }
        [ServiceFilter(typeof(NotFoundFilter<Costumer>))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,CostumerDto costumerDto)
        {
            await _service.UpdateAsync(_mapper.Map<Costumer>(costumerDto));
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }
        [ServiceFilter(typeof(NotFoundFilter<Costumer>))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var costumer = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(costumer);
            return CreateActionResult(ResponseDto<NoContentDto>.Success(204));
        }
    }
}
