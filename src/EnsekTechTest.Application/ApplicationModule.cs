using Autofac;
using EnsekTechTest.Application.CommandHandlers;
using EnsekTechTest.Application.Commands;
using MediatR;

namespace EnsekTechTest.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<AddMeterReadingsToAccountCommandHandler>().AsImplementedInterfaces();
        }
    }
}
