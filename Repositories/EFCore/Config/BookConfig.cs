using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book { Id = 1, Title = "Karagöz ve Hacivat", Price = 18 },
                new Book { Id = 2, Title = "Nasrettin Hoca", Price = 32 },
                new Book { Id = 3, Title = "keloğlan masalları", Price = 18 },
                new Book { Id = 4, Title = "Karagöz ve Hacivat", Price = 18 }
                );
        }
    }
}
