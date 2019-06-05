using Account.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Data.EFConfiguration;

namespace Account.API.Infrastructure.Data.EFConfiguration
{
    public sealed class CustomerConfiguration : Configuration<Customer>
    {
        public override void MapConfigure(EntityTypeBuilder<Customer> customerConfiguration)
        {
            customerConfiguration.Property(c => c.FullName)
                .HasColumnName("fullname")
                .HasColumnType("VARCHAR(80)")
                .IsRequired();

            customerConfiguration.Property(c => c.BirthDate)
                .HasColumnName("birthdate")
                .HasColumnType("DATE")
                .IsRequired();

            customerConfiguration.OwnsOne(c => c.Document, doc =>
            {
                doc.Property(d => d.Text)
                    .HasColumnName("doc_text")
                    .HasColumnType("VARCHAR(11)")
                    .IsRequired();

                doc.Property(d => d.Photo)
                    .HasColumnName("doc_photo")
                    .HasColumnType("TEXT");

                doc.Property(d => d.Verified)
                    .HasColumnName("doc_verified")
                    .HasColumnType("BIT");
            });
        }
    }
}