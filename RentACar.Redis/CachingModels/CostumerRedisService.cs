using AutoMapper;
using RentACar.Core.Models;
using RentACar.Core.Repositories;
using RentACar.Core.Services;
using RentACar.Core.UnitOfWorks;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;

namespace RentACar.Redis.CachingModels
{
    public class CostumerRedisService : ICostumerService, IRedisCaching<Costumer>
    {
        private const string CachingCostumerKey = "costumers";
        private readonly IMapper _mapper;
        private readonly RedisConnectionService _cache;
        private readonly ICostumerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CostumerRedisService(IMapper mapper, RedisConnectionService cache, ICostumerRepository repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _cache = cache;
            _repository = repository;
            _unitOfWork = unitOfWork;
            AddCache();
        }

        public async Task<Costumer> AddAsync(Costumer entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            AddCache();
            return entity;
        }

        public async Task<IEnumerable<Costumer>> AddRangeAsync(IEnumerable<Costumer> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            AddCache();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Costumer, bool>> expression)
        {
            return Task.FromResult(GetCacheDataList().Any(expression.Compile()));
        }

        public Task<IEnumerable<Costumer>> GetAllAsync()
        {
            List<Costumer> list = new List<Costumer>();

            _cache.GetDb(0).ListRange(CachingCostumerKey).ToList().ForEach(x =>
                {
                    Costumer costumer = JsonSerializer.Deserialize<Costumer>(x);
                    list.Add(costumer);
                });
            return Task.FromResult(list.AsEnumerable());
        }

        public Task<Costumer> GetByIdAsync(int id)
        {
            Costumer value = null;
            GetCacheDataList().ForEach(x =>
            {
                if (x.Id == id)
                {
                    value = x;
                }
            });
            return Task.FromResult(value);
        }

        public async Task RemoveAsync(Costumer entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            AddCache();
        }

        public async Task RemoveRangeAsync(IEnumerable<Costumer> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            AddCache();
        }

        public async Task UpdateAsync(Costumer entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            AddCache();
        }
        public IQueryable<Costumer> Where(Expression<Func<Costumer, bool>> expression)
        {
            return GetCacheDataList().Where(expression.Compile()).AsQueryable();
        }
        public List<Costumer> GetCacheDataList()
        {
            List<Costumer> list = new List<Costumer>();

            _cache.GetDb(0).ListRange(CachingCostumerKey).ToList().ForEach(x =>
            {
                Costumer costumer = JsonSerializer.Deserialize<Costumer>(x);
                list.Add(costumer);
            });
            return list;
        }
        public void AddCache()
        {
            if (_cache.GetDb(0).KeyExists(CachingCostumerKey))
            {
                _cache.GetDb(0).KeyDelete(CachingCostumerKey);
            }
            foreach (var item in _repository.GetAll().ToList())
            {
                string jsonEntities = JsonSerializer.Serialize(item);
                _cache.GetDb(0).ListRightPush(CachingCostumerKey, jsonEntities);
            }
            _cache.GetDb(0).KeyExpire(CachingCostumerKey, DateTime.Now.AddMinutes(1));
        }
    }
}
