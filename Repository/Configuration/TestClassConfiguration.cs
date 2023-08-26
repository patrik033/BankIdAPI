using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configuration
{
    public class TestClassConfiguration : IEntityTypeConfiguration<TestClass>
    {
        public void Configure(EntityTypeBuilder<TestClass> builder)
        {
            builder.HasData

                (
                    new TestClass
                    {
                        Id = Guid.NewGuid(),
                        Address = "456, Wall Street"
                    },
                    new TestClass
                    {
                        Id = Guid.NewGuid(),
                        Address = "323, Wyoming street"
                    }
                );
        }
    }
}
