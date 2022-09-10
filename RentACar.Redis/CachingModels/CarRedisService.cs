using AutoMapper;
using Newtonsoft.Json;
using RentACar.Core.Models;
using RentACar.Core.Repositories;
using RentACar.Core.Services;
using RentACar.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RentACar.Redis.CachingModels
{
    public class CarRedisService : ICarService, IRedisCaching<Car>
    {
        private const string CachingCarKey = "cars";
        private readonly IMapper _mapper;
        private readonly RedisConnectionService _cache;
        private readonly ICarRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CarRedisService(IMapper mapper, RedisConnectionService cache, ICarRepository repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _cache = cache;
            _repository = repository;
            _unitOfWork = unitOfWork;
            AddCache();
        }

        public async Task<Car> AddAsync(Car entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            AddCache();
            return entity;
        }

        public async Task<IEnumerable<Car>> AddRangeAsync(IEnumerable<Car> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            AddCache();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Car, bool>> expression)
        {
            return Task.FromResult(GetCacheDataList().Any(expression.Compile()));
        }

        public Task<IEnumerable<Car>> GetAllAsync()
        {
            List<Car> list = new List<Car>();
            _cache.GetDb(0).ListRange(CachingCarKey).ToList().ForEach(x =>
            {
                Car car = JsonConvert.DeserializeObject<Car>(x);
                list.Add(car);
            });
            return Task.FromResult(list.AsEnumerable());
        }

        public Task<Car> GetByIdAsync(int id)
        {
            Car value = null;
            GetCacheDataList().ForEach(x =>
            {
                if (x.Id == id)
                {
                    value = x;
                }
            });
            return Task.FromResult(value);
        }

        public Task<Car> GetByIdCarWithFeatureAsync(int id)
        {
            Car value = null;
            GetCacheDataList().ForEach(x =>
            {
                if (x.Id == id)
                {
                    value = x;
                }
            });
            return Task.FromResult(value);
        }

        public Task<List<Car>> GetCarWithFeatureAsync()
        {
            List<Car> list = new List<Car>();
            _cache.GetDb(0).ListRange(CachingCarKey).ToList().ForEach(x =>
            {
                Car car = JsonConvert.DeserializeObject<Car>(x);
                list.Add(car);
            });
            return Task.FromResult(list);
        }

        public async Task RemoveAsync(Car entity)
        {
            var data = entity;
            _repository.Remove(data);
            await _unitOfWork.CommitAsync();
            AddCache();
        }

        public async Task RemoveRangeAsync(IEnumerable<Car> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            AddCache();
        }

        public async Task UpdateAsync(Car entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            AddCache();
        }

        public IQueryable<Car> Where(Expression<Func<Car, bool>> expression)
        {
            return GetCacheDataList().Where(expression.Compile()).AsQueryable();
        }

        public void AddCache()
        {
            if (_cache.GetDb(0).KeyExists(CachingCarKey))
            {
                _cache.GetDb(0).KeyDelete(CachingCarKey);
            }

            foreach (var item in _repository.GetCarWithFeatureAsync().Result.ToList())
            {
                string jsonEntities = JsonConvert.SerializeObject(item, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                _cache.GetDb(0).ListRightPush(CachingCarKey, jsonEntities);
            }
            _cache.GetDb(0).KeyExpire(CachingCarKey, DateTime.Now.AddMinutes(1));
        }

        public List<Car> GetCacheDataList()
        {
            List<Car> list = new List<Car>();
            _cache.GetDb(0).ListRange(CachingCarKey).ToList().ForEach(x =>
            {
                Car car = JsonConvert.DeserializeObject<Car>(x);
                list.Add(car);
            });
            return list;
        }
    }
}
