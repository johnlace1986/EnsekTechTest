using Autofac;
using EnsekTechTest.Application.Infrastructure;
using EnsekTechTest.Application.Infrastructure.Repositories;
using EnsekTechTest.Persistence.DbContexts;
using EnsekTechTest.Persistence.Repositories;

namespace EnsekTechTest.Persistence
{
    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<PersistenceContext>().AsImplementedInterfaces();

            builder.RegisterType<AccountsRepository>().As<IAccountsRepository>();
            builder.RegisterType<MeterReadingsRepository>().As<IMeterReadingsRepository>();
        }
    }
}
