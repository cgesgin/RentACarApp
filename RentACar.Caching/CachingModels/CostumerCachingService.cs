using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentACar.Core.Models;
using RentACar.Core.Repositories;
using RentACar.Core.Services;
using RentACar.Core.UnitOfWorks;
using RentACar.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Caching.CachingModels
{
    public class CostumerCachingService : ICostumerService
    {
        private const string CachingCostumerKey = "costumers";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly ICostumerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CostumerCachingService(IMapper mapper, IMemoryCache memoryCache, ICostumerRepository repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _repository = repository;
            _unitOfWork = unitOfWork;
            if (!_memoryCache.TryGetValue(CachingCostumerKey, out _))
            {
                _memoryCache.Set(CachingCostumerKey, _repository.GetAll().ToList());
            }
        }

        public async Task<Costumer> AddAsync(Costumer entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllAsync();
            return entity;
        }

        public async Task<IEnumerable<Costumer>> AddRangeAsync(IEnumerable<Costumer> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllAsync();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Costumer, bool>> expression)
        {
            return Task.FromResult(_memoryCache.Get<List<Costumer>>(CachingCostumerKey).Any(expression.Compile()));
        }

        public Task<IEnumerable<Costumer>> GetAllAsync()
        {
            var costumers = _memoryCache.Get<IEnumerable<Costumer>>(CachingCostumerKey);
            return Task.FromResult(costumers);
        }

        public Task<Costumer> GetByIdAsync(int id)
        {
            var Isthere =  _memoryCache.Get<List<Costumer>>(CachingCostumerKey).FirstOrDefault(x => x.Id == id);
            if (Isthere == null)
            {
                throw new NotFoundExcepiton($"{typeof(Costumer).Name} data not found");
            }
            return Task.FromResult(Isthere);
        }

        public async Task RemoveAsync(Costumer entity)
        {
            var Isthere = await _repository.AnyAsync(x => x.Id == entity.Id);
            if (!Isthere)
            {
                throw new NotFoundExcepiton($"{typeof(Costumer).Name} data not found");
            }
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Costumer> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllAsync();
        }

        public async Task UpdateAsync(Costumer entity)
        {
            var Isthere = await _repository.AnyAsync(x=>x.Id==entity.Id);
            if (!Isthere)
            {
                throw new NotFoundExcepiton($"{typeof(Costumer).Name} data not found");
            }
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllAsync();
        }

        public IQueryable<Costumer> Where(Expression<Func<Costumer, bool>> expression)
        {
            return _memoryCache.Get<List<Costumer>>(CachingCostumerKey).Where(expression.Compile()).AsQueryable();
        }
        public async Task CacheAllAsync()
        {
            _memoryCache.Set(CachingCostumerKey, await _repository.GetAll().ToListAsync());
        }
    }
}
