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
        public virtual DbSet<Person> persons { get; set; } 

        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("countries");
            modelBuilder.Entity<Person>().ToTable("Person");

            //seed to Countries
            string countriesJson = System.IO.File.ReadAllText("countries.json");
            List<Country> countries=System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

            foreach(Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            //Seed to persons
            string PerosnJson = System.IO.File.ReadAllText("persons.json");
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(PerosnJson);

            foreach (Person person in persons)
                modelBuilder.Entity<Person>().HasData(person);

            //Table Relations
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasOne<Country>(c => c.Country)
                   .WithMany(p => p.Persons)
                   .HasForeignKey(p => p.CountryID);
            });
        }
   
    }
}
