using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
	public void Configure(EntityTypeBuilder<Product> builder)
	{
		builder.HasData
		(
			new Product()
			{
				Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
				Name = "Intel Core",
				Description = "CPU model"
			},
			new Product()
			{
				Id = new Guid("c9d4c053-49b6-410c-bc68-2d54a9991870"),
				Name = "Apple Desktop",
				Description = "2006 model"
			}
		);
	}
}