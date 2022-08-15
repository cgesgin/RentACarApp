using RentACar.Core.DTOs;
using RentACar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.Services
{
    public interface IModelService : IService<Model>
    {
        Task<ResponseDto<List<ModelWithBrandDto>>> GetModelsWithBrandAsync();
    }
}
