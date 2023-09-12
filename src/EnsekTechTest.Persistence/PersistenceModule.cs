using Autofac;
using EnsekTechTest.Application.Repositories;
using EnsekTechTest.Persistence.Repositories;

namespace EnsekTechTest.Persistence
{
    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<AccountsRepository>().As<IAccountsRepository>();
            builder.RegisterType<MeterReadingsRepository>().As<IMeterReadingsRepository>();
        }
    }
}
