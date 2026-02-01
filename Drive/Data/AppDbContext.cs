using Microsoft.EntityFrameworkCore;
using Drive.Models;
using System.Collections.Generic;

namespace Drive.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<ArquivoModel> Arquivos { get; set; }
    }
}
