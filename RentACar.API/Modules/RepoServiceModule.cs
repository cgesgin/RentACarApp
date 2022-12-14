using Autofac;
using RentACar.Caching;
using RentACar.Caching.CachingModels;
using RentACar.Core.Repositories;
using RentACar.Core.Services;
using RentACar.Core.UnitOfWorks;
using RentACar.Redis.CachingModels;
using RentACar.Repository;
using RentACar.Repository.Repositories;
using RentACar.Repository.UnitOfWorks;
using RentACar.Service.Services;
using System.Reflection;

namespace RentACar.API.Modules
{
    public class RepoServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();


            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            var apiAssembly = Assembly.GetExecutingAssembly();
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
            var serviceAssembly= Assembly.GetAssembly(typeof(Service<>));

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly)
                .Where(x => x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly)
                .Where(x => x.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Open The Caching
            //builder.RegisterType<CostumerCachingService>().As<ICostumerService>();
            //builder.RegisterType<CarCachingService>().As<ICarService>();

            // Open The Redis
            //builder.RegisterType<CostumerRedisService>().As<ICostumerService>();
            //builder.RegisterType<CarRedisService>().As<ICarService>();

            base.Load(builder);
        }
    }
}
