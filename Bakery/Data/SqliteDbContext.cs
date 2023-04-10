using Bakery.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Data
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<IngredientPrice> IngredientPrices { get; set; }
        public DbSet<RecipeRecipe> RecipeRecipes { get; set; }

        public string DbPath { get; }

        public SqliteDbContext()
        {
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            //var path = Environment.GetFolderPath();
            //DbPath = System.IO.Path.Join("C:\\BakeryApp", "bakery.db");
            DbPath = System.IO.Path.Join("C:\\Users\\Gebruiker\\OneDrive\\bakkerij simon en katlijn\\BakeryApp", "bakery.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
                => options.UseSqlite($"Data Source={DbPath}");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Ingredients)
                .WithOne();
            modelBuilder.Entity<Recipe>()
                .Navigation(b => b.Ingredients)
                .UsePropertyAccessMode(PropertyAccessMode.Property);
            modelBuilder.Entity<RecipeIngredient>()
            .HasOne(p => p.Recipe)
            .WithMany(b => b.Ingredients)
            .HasForeignKey(p => p.RecipeID);

            modelBuilder.Entity<Ingredient>()
                .HasMany(r => r.Recipes)
                .WithOne();
            modelBuilder.Entity<Ingredient>()
                .Navigation(b => b.Recipes)
                .UsePropertyAccessMode(PropertyAccessMode.Property);
            modelBuilder.Entity<RecipeIngredient>()
            .HasOne(p => p.Ingredient)
            .WithMany(b => b.Recipes)
            .HasForeignKey(p => p.IngredientID);

            modelBuilder.Entity<Ingredient>()
                .HasMany(r => r.Prices)
                .WithOne();
            modelBuilder.Entity<Ingredient>()
                .Navigation(b => b.Prices)
                .UsePropertyAccessMode(PropertyAccessMode.Property);
            modelBuilder.Entity<IngredientPrice>()
            .HasOne(p => p.Ingredient)
            .WithMany(b => b.Prices)
            .HasForeignKey(p => p.IngredientID);

            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.InBetweenRecipes)
                .WithOne();
            modelBuilder.Entity<Recipe>()
                .Navigation(b => b.InBetweenRecipes)
                .UsePropertyAccessMode(PropertyAccessMode.Property);
            modelBuilder.Entity<RecipeRecipe>()
            .HasOne(p => p.FullRecipe)
            .WithMany(b => b.InBetweenRecipes)
            .HasForeignKey(p => p.FullRecipeID);
            modelBuilder.Entity<RecipeRecipe>()
            .HasOne(p => p.InBetweenRecipe)
            .WithMany(b => b.PartOfRecipes)
            .HasForeignKey(p => p.InBetweenRecipeID);

            modelBuilder.Entity<Recipe>().HasKey(x => x.RecipeID);
            modelBuilder.Entity<Ingredient>().HasKey(x => x.IngredientID);
            modelBuilder.Entity<RecipeRecipe>().HasKey(x => new { x.InBetweenRecipeID, x.FullRecipeID });
            modelBuilder.Entity<RecipeIngredient>().HasKey(x => new { x.RecipeID, x.IngredientID });
            modelBuilder.Entity<IngredientPrice>().HasKey(x => new { x.IngredientID, x.From });
        }
    }
}
