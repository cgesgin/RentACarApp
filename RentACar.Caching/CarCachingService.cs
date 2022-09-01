using Microsoft.Extensions.Caching.Memory;
using RentACar.Core.Models;
using RentACar.Core.Repositories;
using RentACar.Core.Services;
using RentACar.Core.UnitOfWorks;
using RentACar.Service.Exceptions;
using System.Linq.Expressions;

namespace RentACar.Caching
{
    public class CarCachingService : ICarService
    {
        private const string CachingCarKey = "cars";
        private readonly IMemoryCache _memoryCache;
        private readonly ICarRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CarCachingService(IMemoryCache memoryCache, ICarRepository repository, IUnitOfWork unitOfWork)
        {
            _memoryCache = memoryCache;
            _repository = repository;
            _unitOfWork = unitOfWork;
            if (!_memoryCache.TryGetValue(CachingCarKey, out _))
            {
                _memoryCache.Set(CachingCarKey, _repository.GetCarWithFeatureAsync().Result.ToList());
            }
        }

        public async Task<Car> AddAsync(Car entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllAsync();
            return entity;
        }

        public async Task<IEnumerable<Car>> AddRangeAsync(IEnumerable<Car> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllAsync();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Car, bool>> expression)
        {
            return Task.FromResult(_memoryCache.Get<List<Car>>(CachingCarKey).Any(expression.Compile()));
        }

        public Task<IEnumerable<Car>> GetAllAsync()
        {
            var cars = _memoryCache.Get<IEnumerable<Car>>(CachingCarKey);
            return Task.FromResult(cars);
        }

        public Task<Car> GetByIdAsync(int id)
        {
            var Isthere = _memoryCache.Get<List<Car>>(CachingCarKey).FirstOrDefault(x => x.Id == id);
            if (Isthere == null)
            {
                throw new NotFoundExcepiton($"{typeof(Car).Name} data not found");
            }
            return Task.FromResult(Isthere);
        }

        public Task<Car> GetByIdCarWithFeatureAsync(int id)
        {
            var Isthere = _memoryCache.Get<List<Car>>(CachingCarKey).FirstOrDefault(x => x.Id == id);
            if (Isthere == null)
            {
                throw new NotFoundExcepiton($"{typeof(Car).Name} data not found");
            }
            return Task.FromResult(Isthere);
        }

        public Task<List<Car>> GetCarWithFeatureAsync()
        {
            var cars = _memoryCache.Get<List<Car>>(CachingCarKey);
            return Task.FromResult(cars);
        }

        public async Task RemoveAsync(Car entity)
        {
            var Isthere = await _repository.AnyAsync(x => x.Id == entity.Id);
            if (!Isthere)
            {
                throw new NotFoundExcepiton($"{typeof(Car).Name} data not found");
            }
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Car> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllAsync();
        }

        public async Task UpdateAsync(Car entity)
        {
            var Isthere = await _repository.AnyAsync(x => x.Id == entity.Id);
            if (!Isthere)
            {
                throw new NotFoundExcepiton($"{typeof(Car).Name} data not found");
            }
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllAsync();
        }

        public IQueryable<Car> Where(Expression<Func<Car, bool>> expression)
        {
            return _memoryCache.Get<List<Car>>(CachingCarKey).Where(expression.Compile()).AsQueryable();
        }
        public async Task CacheAllAsync()
        {
            _memoryCache.Set(CachingCarKey, _repository.GetCarWithFeatureAsync().Result.ToList());
        }
    }
}
