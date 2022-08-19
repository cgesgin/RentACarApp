using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RentACar.Core.DTOs;
using RentACar.Core.Models;
using RentACar.Core.Services;
using RentACar.Web.MVC.Filters;

namespace RentACar.Web.MVC.Controllers
{
    public class CostumersController : Controller
    {
        private readonly ICostumerService _service; 
        private readonly IMapper _mapper;
        public CostumersController(ICostumerService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _service.GetAllAsync();
            return View(_mapper.Map<List<CostumerDto>>(response));
        }
        public IActionResult Save()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save(CostumerDto costumerDto)
        {
            if (ModelState.IsValid)
            {
                await _service.AddAsync(_mapper.Map<Costumer>(costumerDto));
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [ServiceFilter(typeof(NotFoundFilter<Costumer>))]
        public async Task<IActionResult> Update(int id)
        {
            var costumer = await _service.GetByIdAsync(id);
            return View(_mapper.Map<CostumerDto>(costumer));
        }
       
        [HttpPost]
        public async Task<IActionResult> Update(CostumerDto costumerDto)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(_mapper.Map<Costumer>(costumerDto));
                return RedirectToAction(nameof(Index));
            }           
            return View(costumerDto);
        }
        public async Task<IActionResult> Remove(int id)
        {
            var costumer = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(costumer);
            return RedirectToAction(nameof(Index));
        }
    }
}
