using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyDiet_API.Shared.Models;

namespace MyDiet_API.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<DietMeal>().HasKey(dm => new { dm.DietId, dm.MealId });
            builder.Entity<MealProduct>().HasKey(mp => new { mp.MealId, mp.ProductId });
            builder.Entity<Weight>().Property(w => w.WeightValue).HasColumnType("decimal(2,0)");

            builder.Entity<ProductCategory>().HasIndex(pc => pc.Description).IsUnique();
            builder.Entity<Product>().HasIndex(p => p.Name).IsUnique();
            builder.Entity<Patient>().HasIndex(p => p.FiscalCode).IsUnique();
            builder.Entity<Patient>().HasIndex(p => p.Email).IsUnique();
            builder.Entity<Patient>().HasIndex(p => p.Phone).IsUnique();
            builder.Entity<Meal>().HasIndex(m => m.Name).IsUnique();
            builder.Entity<Diet>().HasIndex(d => d.Name).IsUnique();
        }

        public DbSet<Diet> Diets { get; set; }
        public DbSet<DietMeal> DietMeals { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealProduct> MealProducts { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Weight> Weights { get; set; }
    }
}
