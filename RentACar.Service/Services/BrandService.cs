using AutoMapper;
using RentACar.Core.DTOs;
using RentACar.Core.Models;
using RentACar.Core.Repositories;
using RentACar.Core.Services;
using RentACar.Core.UnitOfWorks;
using RentACar.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Service.Services
{
    public class BrandService : Service<Brand>, IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public BrandService(IGenericRepository<Brand> repository, IUnitOfWork unitOfWork, IMapper mapper, IBrandRepository brandRepository) : base(repository, unitOfWork)
        {
            _mapper = mapper; 
            _brandRepository = brandRepository;
            _unitOfWork=unitOfWork;
        }

        public async Task<ResponseDto<BrandWithModelsDto>> GetByIdBrandWithModelsAsync(int brandId)
        {
            var brand = await _brandRepository.GetByIdBrandWithModelsAsync(brandId);
            var brandDto = _mapper.Map<BrandWithModelsDto>(brand);
            return ResponseDto<BrandWithModelsDto>.Success(200, brandDto);
        }
        
        public async Task UpdateAsync(Brand entity)
        {
            var obj = await _brandRepository.GetByIdAsync(entity.Id);
            if (obj == null)
            {
                throw new NotFoundExcepiton($"{typeof(Brand).Name}({entity.Id}) not found");
            }
            _brandRepository.Update(entity);
            await _unitOfWork.CommitAsync();
        }


    }
}
