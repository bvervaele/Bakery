using Bakery.Data;
using Bakery.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bakery.MVVM.View
{
    /// <summary>
    /// Interaction logic for RecipeView.xaml
    /// </summary>
    public partial class RecipeView : Window
    {
        public List<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>() { };

        public List<RecipeRecipe> Recipes { get; set; } = new List<RecipeRecipe>() { };

        public int RecipeId { get; set; }

        public SqliteDbContext dbContext { get; set; }

        public List<string> IngredientsNames { get; set; } = new List<string>() { };
        public List<string> InBetweenRecipesNames { get; set; } = new List<string>() { };

        public List<Ingredient> IngredientsOptions { get; set; } = new List<Ingredient>() { };
        public List<Recipe> InBetweenRecipesOptions { get; set; } = new List<Recipe>() { };


        public RecipeView(int id)
        {
            InitializeComponent();
            dbContext = new SqliteDbContext();
            RecipeId = id;

            
            UpdateTable();

            this.ingredients.CellEditEnding += Ingredients_CellEditEnding;
            this.ingredients.PreviewKeyDown += Ingredients_DeletingItem;

            this.inbetweenRecipes.CellEditEnding += Recipes_CellEditEnding;
            this.inbetweenRecipes.PreviewKeyDown += Recipes_DeletingItem;
        }

        public void UpdateTable()
        {
            dbContext.Recipes.Include(x => x.InBetweenRecipes).Include(x => x.Ingredients).ThenInclude(x => x.Ingredient).ThenInclude(x => x.Prices).Load<Recipe>();
            Recipe recipe = dbContext.Recipes.SingleOrDefault(x => x.RecipeID == RecipeId);
            Ingredients = recipe.Ingredients?.ToList();
            Recipes = recipe.InBetweenRecipes?.ToList();
            this.recipeName.Content = recipe.Name;
            this.ingredients.ItemsSource = Ingredients;
            this.ingredients.Items.Refresh();
            this.inbetweenRecipes.ItemsSource = Recipes;
            this.inbetweenRecipes.Items.Refresh();

            var AllInBetweenRecipes = dbContext.Recipes.Where(x => x.Category == RecipeCategory.TussenProduct && x.RecipeID != this.RecipeId).ToList();
            InBetweenRecipesOptions = AllInBetweenRecipes.Except(Recipes.Select(x => x.InBetweenRecipe)).ToList();
            InBetweenRecipesNames = AllInBetweenRecipes.Select(x => x.Name).ToList();
            var AllIngredients = dbContext.Ingredients.ToList();
            IngredientsOptions = AllIngredients.Except(Ingredients.Select(x => x.Ingredient)).ToList();
            IngredientsNames = AllIngredients.Select(x => x.Name).ToList();
            this.inbetweenRecipesNames.ItemsSource = InBetweenRecipesNames;
            this.ingredientsNames.ItemsSource = IngredientsNames;
        }

        private void Ingredients_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                RecipeIngredient selected = e.Row.DataContext as RecipeIngredient;
                dbContext.RecipeRecipes.Include(x => x.InBetweenRecipe);
                var result = dbContext.RecipeIngredients.SingleOrDefault(x => x.RecipeID == RecipeId && x.IngredientID == selected.IngredientID);

                if (result == null)
                {
                    try
                    {
                        var ingredient = IngredientsOptions.FirstOrDefault();
                        if (ingredient == null)
                            return;
                        result = new RecipeIngredient() { RecipeID = RecipeId, IngredientID = ingredient.IngredientID};
                        dbContext.RecipeIngredients.Add(result);
                        dbContext.SaveChanges();
                        
                    }
                    catch (Exception ex)
                    {
                        UpdateTable();
                        return;
                    }
                }

                if (e.Column.Header.Equals("Naam"))
                {
                    var el = e.EditingElement as ComboBox;
                    var Ingrientame = el.SelectedValue.ToString();

                    if (Ingredients.Where(x => x.Name == Ingrientame).Any())
                        return;

                    var IngredientID = dbContext.Ingredients.FirstOrDefault(x => x.Name == Ingrientame).IngredientID;


                    var newRecipeIngredient = new RecipeIngredient() { IngredientID = result.IngredientID, RecipeID = this.RecipeId, Amount = result.Amount };
                    dbContext.RecipeIngredients.Remove(result);
                    try { dbContext.SaveChanges(); } catch { }

                    newRecipeIngredient.IngredientID = IngredientID;

                    dbContext.RecipeIngredients.Add(newRecipeIngredient);
                    try { dbContext.SaveChanges(); } catch { }
                }

                if (e.Column.Header.Equals("Hoeveelheid"))
                {
                    var el = e.EditingElement as TextBox;
                    result.Amount = (double)double.Parse(el.Text.Replace('.', ','));
                    try{ dbContext.SaveChanges(); } catch { }
                }
                UpdateTable();
            }
        }

        private void Recipes_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                RecipeRecipe selected = e.Row.DataContext as RecipeRecipe;
                dbContext.RecipeRecipes.Include(x => x.InBetweenRecipe);
                var result = dbContext.RecipeRecipes.SingleOrDefault(x => x.FullRecipeID == RecipeId && x.InBetweenRecipeID == selected.InBetweenRecipeID);

                if (result == null)
                {
                    try
                    {
                        var tussenRecept = InBetweenRecipesOptions.FirstOrDefault();
                        if (tussenRecept == null)
                            return;

                        result = new RecipeRecipe() { FullRecipeID = RecipeId, InBetweenRecipeID = tussenRecept.RecipeID };
                        dbContext.RecipeRecipes.Add(result);
                        try { dbContext.SaveChanges(); } catch { }
                    }
                    catch (Exception)
                    {
                        UpdateTable();
                        return;
                    }
                }

                if (e.Column.Header.Equals("Naam"))
                {
                    var el = e.EditingElement as ComboBox;
                    var InBetweenRecipeName = el.SelectedValue.ToString();

                    if (Recipes.Where(x => x.Name == InBetweenRecipeName).Any())
                        return;

                    var InBetweenRecipeID = dbContext.Recipes.FirstOrDefault(x => x.Name == InBetweenRecipeName).RecipeID;

                    var newRecipeRecipe = new RecipeRecipe() { InBetweenRecipeID = result.InBetweenRecipeID, FullRecipeID = this.RecipeId, Amount = result.Amount };
                    dbContext.RecipeRecipes.Remove(result);
                    try { dbContext.SaveChanges(); } catch { }

                    newRecipeRecipe.InBetweenRecipeID = InBetweenRecipeID;

                    dbContext.RecipeRecipes.Add(newRecipeRecipe);
                    try { dbContext.SaveChanges(); } catch { }
                }

                if (e.Column.Header.Equals("Hoeveelheid"))
                {
                    var el = e.EditingElement as TextBox;
                    result.Amount = (double) double.Parse(el.Text.Replace('.',','));
                    try { dbContext.SaveChanges(); } catch { }
                }
                UpdateTable();
            }
        }

        private void Ingredients_DeletingItem(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var grid = (DataGrid)sender;
                foreach (var row in grid.SelectedItems)
                {
                    try
                    {
                        Ingredient ingredient = row as Ingredient;
                        dbContext.Ingredients.Remove(ingredient);
                        dbContext.SaveChanges();
                    }
                    catch (Exception) { }
                }
                UpdateTable();
            }
        }

        private void Recipes_DeletingItem(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var grid = (DataGrid)sender;
                foreach (var row in grid.SelectedItems)
                {
                    try
                    {
                        Recipe recipe = row as Recipe;
                        dbContext.Recipes.Remove(recipe);
                        dbContext.SaveChanges();
                    }
                    catch (Exception) { }
                }
                UpdateTable();
            }
        }
    }
}
