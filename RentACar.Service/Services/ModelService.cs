using AutoMapper;
using RentACar.Core.DTOs;
using RentACar.Core.Models;
using RentACar.Core.Repositories;
using RentACar.Core.Services;
using RentACar.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Service.Services
{
    public class ModelService : Service<Model>, IModelService
    {
        private readonly IModelRepository _modelRepository;
        private readonly IMapper _mapper;
        public ModelService(IGenericRepository<Model> repository, IUnitOfWork unitOfWork, IModelRepository modelRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }

        //for Api response Data
        public async Task<ResponseDto<List<ModelWithBrandDto>>> GetModelsWithBrandAsync()
        {
            var carModel = await _modelRepository.GetModelsWithBrandAsync();
            var carmodelDtos= _mapper.Map<List<ModelWithBrandDto>>(carModel);
            return ResponseDto<List<ModelWithBrandDto>>.Success(200, carmodelDtos);
        }
    }
}
