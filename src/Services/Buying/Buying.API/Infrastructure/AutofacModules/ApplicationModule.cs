using Autofac;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.Services.Buying.API.Application.Commands;
using Microsoft.eShopOnContainers.Services.Buying.API.Application.Queries;
using Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.BuyerAggregate;
//using Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.OrderAggregate;
using Microsoft.eShopOnContainers.Services.Buying.Infrastructure.Idempotency;
using Microsoft.eShopOnContainers.Services.Buying.Infrastructure.Repositories;
using System.Reflection;

namespace Microsoft.eShopOnContainers.Services.Buying.API.Infrastructure.AutofacModules
{

    public class ApplicationModule
        : Autofac.Module
    {

        public string QueriesConnectionString { get; }

        public ApplicationModule(string qconstr)
        {
            QueriesConnectionString = qconstr;

        }

        protected override void Load(ContainerBuilder builder)
        {

            builder.Register(c => new BuyerQueries(QueriesConnectionString))
                .As<IBuyerQueries>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BuyerRepository>()
                .As<IBuyerRepository>()
                .InstancePerLifetimeScope();

            //builder.RegisterType<OrderRepository>()
            //    .As<IOrderRepository>()
            //    .InstancePerLifetimeScope();

            builder.RegisterType<RequestManager>()
               .As<IRequestManager>()
               .InstancePerLifetimeScope();

            //builder.RegisterAssemblyTypes(typeof(CreateOrderCommandHandler).GetTypeInfo().Assembly)
            //    .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));

        }
    }
}
