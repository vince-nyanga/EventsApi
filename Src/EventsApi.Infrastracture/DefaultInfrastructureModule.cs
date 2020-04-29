using System.Reflection;
using Autofac;
using EventsApi.Core.Events;
using EventsApi.Core.Interfaces;
using EventsApi.Infrastracture.Data;
using EventsApi.Infrastracture.Services;
using MediatR;
using Module = Autofac.Module;

namespace EventsApi.Infrastracture
{
    public class DefaultInfrastructureModule : Module
    {
        private readonly bool _isDevelopment;

        public DefaultInfrastructureModule(bool isDevelopment)
        {
            _isDevelopment = isDevelopment;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_isDevelopment)
            {
                LoadDevelopmentOnlyDependencies(builder);
            }
            else
            {
                LoadProductionOnlyDependencies(builder);
            }

            LoadCommonDependencies(builder);
        }

        private void LoadDevelopmentOnlyDependencies(ContainerBuilder builder)
        {
            // TODO: Load development dependencies
        }

        private void LoadProductionOnlyDependencies(ContainerBuilder builder)
        {
            // TODO: Load production dependencies
        }

        private void LoadCommonDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<LocalSmtpEmailService>().As<IEmailService>();

            builder.RegisterType<MediatorDomainEventDispatcher>().As<IDomainEventDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EfRepository>().As<IRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(NewTalkAddedEvent).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));

        }
    }
}
