using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Repository.Seed
{
    internal class ModelSeed : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.HasData(
               new Model { Id = 1, Name = "X1", BrandId = 1 },
               new Model { Id = 2, Name = "X2", BrandId = 1 },
               new Model { Id = 3, Name = "A1", BrandId = 2 },
               new Model { Id = 4, Name = "A2", BrandId = 2 },
               new Model { Id = 5, Name = "A2", BrandId = 3 }
               );
        }
    }
}
