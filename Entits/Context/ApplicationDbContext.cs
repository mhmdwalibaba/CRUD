using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Entits
{
    public class ApplicationDbContext:DbContext
    {
        public virtual DbSet<Country> countries { get; set; }

        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("countries");

            //seed to Countries
            string countriesJson = System.IO.File.ReadAllText("countries.json");
            List<Country> countries=System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

            foreach(Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }
        }
    }
}
