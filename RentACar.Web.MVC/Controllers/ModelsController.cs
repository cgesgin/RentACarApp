using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Core.DTOs;
using RentACar.Core.Models;
using RentACar.Core.Services; 

namespace RentACar.Web.MVC.Controllers
{
    public class ModelsController : Controller
    {
        private readonly IModelService _service;
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;
        public ModelsController(IModelService service, IBrandService brandService, IMapper mapper)
        {
            _service = service;
            _brandService = brandService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _service.GetModelsWithBrandAsync();
            return View(response.Data);
        }
        public async Task<IActionResult> Save()
        {
            var brands = await _brandService.GetAllAsync();
            var brandDto = _mapper.Map<List<BrandDto>>(brands.ToList());
            ViewBag.brands = new SelectList(brandDto,"Id","Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save(ModelDto modelDto)
        {
            if (ModelState.IsValid)
            {
                await _service.AddAsync(_mapper.Map<Model>(modelDto));
                return RedirectToAction(nameof(Index));
            }
            var brands = await _brandService.GetAllAsync();
            var brandDto = _mapper.Map<List<BrandDto>>(brands.ToList());
            ViewBag.brands = new SelectList(brandDto, "Id", "Name");
            return View();
        }

        public async Task<IActionResult> Update(int id) {
            var carModel =await _service.GetByIdAsync(id);
            var brands = await _brandService.GetAllAsync();
            var brandDto = _mapper.Map<List<BrandDto>>(brands.ToList());
            ViewBag.brands = new SelectList(brandDto, "Id", "Name", carModel.BrandId);
            return View(_mapper.Map<ModelDto>(carModel));
        }
        [HttpPost]
        public async Task<IActionResult> Update(ModelDto modelDto)
        {
            if (ModelState.IsValid) 
            {
                await _service.UpdateAsync(_mapper.Map<Model>(modelDto));
                return RedirectToAction(nameof(Index));
            }
            var brands = await _brandService.GetAllAsync();
            var brandDto = _mapper.Map<List<BrandDto>>(brands.ToList());
            ViewBag.brands = new SelectList(brandDto, "Id", "Name", modelDto.BrandId);
            return View(modelDto);
        }
        public async Task<IActionResult> Remove(int id) 
        {
            var carModel = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(carModel);
            return RedirectToAction(nameof(Index));
        }
    }
}
