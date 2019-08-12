using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.BuyerAggregate;
using Microsoft.eShopOnContainers.Services.Buying.Infrastructure;

namespace Buying.Infrastructure.EntityConfigurations
{
    class BuyerEntityTypeConfiguration
        : IEntityTypeConfiguration<Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.BuyerAggregate.Buyer>
    {
        public void Configure(EntityTypeBuilder<Microsoft.eShopOnContainers.Services.Buying.Domain.AggregatesModel.BuyerAggregate.Buyer> buyerConfiguration)
        {
            buyerConfiguration.ToTable("buyers", BuyingContext.DEFAULT_SCHEMA);

            buyerConfiguration.HasKey(b => b.Id);

            buyerConfiguration.Ignore(b => b.DomainEvents);

            buyerConfiguration.Property(b => b.Id)
                .ForSqlServerUseSequenceHiLo("buyerseq", BuyingContext.DEFAULT_SCHEMA);

            buyerConfiguration.Property(b => b.IdentityGuid)
                .HasMaxLength(200)
                .IsRequired();

            buyerConfiguration.HasIndex("IdentityGuid")
              .IsUnique(true);

            buyerConfiguration.HasMany(b => b.PaymentMethods)
               .WithOne()
               .HasForeignKey("BuyerId")
               .OnDelete(DeleteBehavior.Cascade);

            var navigation = buyerConfiguration.Metadata.FindNavigation(nameof(Buyer.PaymentMethods));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
