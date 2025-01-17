﻿// <auto-generated />
using System;
using Bakery.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bakery.Migrations
{
    [DbContext(typeof(SqliteDbContext))]
    partial class SqliteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("Bakery.Models.Ingredient", b =>
                {
                    b.Property<int>("IngredientID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Unit")
                        .HasColumnType("INTEGER");

                    b.HasKey("IngredientID");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("Bakery.Models.IngredientPrice", b =>
                {
                    b.Property<int>("IngredientID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("From")
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.HasKey("IngredientID", "From");

                    b.ToTable("IngredientPrices");
                });

            modelBuilder.Entity("Bakery.Models.Recipe", b =>
                {
                    b.Property<int>("RecipeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Category")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Pieces")
                        .HasColumnType("INTEGER");

                    b.Property<double>("WorkingMinutes")
                        .HasColumnType("REAL");

                    b.HasKey("RecipeID");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("Bakery.Models.RecipeIngredient", b =>
                {
                    b.Property<int>("RecipeID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IngredientID")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.HasKey("RecipeID", "IngredientID");

                    b.HasIndex("IngredientID");

                    b.ToTable("RecipeIngredients");
                });

            modelBuilder.Entity("Bakery.Models.RecipeRecipe", b =>
                {
                    b.Property<int>("InBetweenRecipeID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FullRecipeID")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.HasKey("InBetweenRecipeID", "FullRecipeID");

                    b.HasIndex("FullRecipeID");

                    b.ToTable("RecipeRecipes");
                });

            modelBuilder.Entity("Bakery.Models.IngredientPrice", b =>
                {
                    b.HasOne("Bakery.Models.Ingredient", "Ingredient")
                        .WithMany("Prices")
                        .HasForeignKey("IngredientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ingredient");
                });

            modelBuilder.Entity("Bakery.Models.RecipeIngredient", b =>
                {
                    b.HasOne("Bakery.Models.Ingredient", "Ingredient")
                        .WithMany("Recipes")
                        .HasForeignKey("IngredientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bakery.Models.Recipe", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ingredient");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Bakery.Models.RecipeRecipe", b =>
                {
                    b.HasOne("Bakery.Models.Recipe", "FullRecipe")
                        .WithMany("InBetweenRecipes")
                        .HasForeignKey("FullRecipeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bakery.Models.Recipe", "InBetweenRecipe")
                        .WithMany("PartOfRecipes")
                        .HasForeignKey("InBetweenRecipeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FullRecipe");

                    b.Navigation("InBetweenRecipe");
                });

            modelBuilder.Entity("Bakery.Models.Ingredient", b =>
                {
                    b.Navigation("Prices");

                    b.Navigation("Recipes");
                });

            modelBuilder.Entity("Bakery.Models.Recipe", b =>
                {
                    b.Navigation("InBetweenRecipes");

                    b.Navigation("Ingredients");

                    b.Navigation("PartOfRecipes");
                });
#pragma warning restore 612, 618
        }
    }
}
