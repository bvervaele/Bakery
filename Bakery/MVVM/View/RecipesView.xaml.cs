using Bakery.Data;
using Bakery.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Bakery.MVVM.View
{
    /// <summary>
    /// Interaction logic for RecipesView.xaml
    /// </summary>
    public partial class RecipesView : UserControl
    {
        public List<Recipe> Recipes { get; set; }

        public SqliteDbContext dbContext { get; set; }

        public string _filterName = "";

        public string _filterCategory = "Alles";

        public DateTime _filterDate = DateTime.Now;

        private List<string> _categories = new List<string>() { "Alles", "TussenProduct", "Taart", "DroogGebak", "Brood", "Pistolets", "KoffieKoeken", "Koeken" };

        public RecipesView()
        {
            InitializeComponent();
            dbContext = new SqliteDbContext();

            UpdateTable();

            _categories.ForEach(x => this.categories.Items.Add(x));
            this.categories.SelectedItem = "Alles";

            this.categories.SelectionChanged += Recipes_CategoryChanged;
            this.datePicker.SelectedDateChanged += Recipes_DateChanged;
            this.recipes.CellEditEnding += Recipes_CellEditEnding;
            this.recipes.PreviewKeyDown += Recipes_DeletingItem;
            this.recipes.MouseDoubleClick += Recipes_DoubleClick;
        }

        public void UpdateTable()
        {
            dbContext.Recipes.Include(x => x.InBetweenRecipes).Include(x => x.Ingredients).ThenInclude(x => x.Ingredient).ThenInclude(x => x.Prices).Load<Recipe>();
            Recipes = dbContext.Recipes.OrderBy(x => x.Name).ToList();
            if (_filterName != "")
                Recipes = Recipes.Where(x => x.Name.StartsWith(_filterName)).ToList();
            switch (_filterCategory)
            {
                case "TussenProduct":
                    Recipes = Recipes.Where(x => x.Category == RecipeCategory.TussenProduct).ToList();
                    break;
                case "Taart":
                    Recipes = Recipes.Where(x => x.Category == RecipeCategory.Taart).ToList();
                    break;
                case "DroogGebak":
                    Recipes = Recipes.Where(x => x.Category == RecipeCategory.DroogGebak).ToList();
                    break;
                case "Brood":
                    Recipes = Recipes.Where(x => x.Category == RecipeCategory.Brood).ToList();
                    break;
                case "Pistolets":
                    Recipes = Recipes.Where(x => x.Category == RecipeCategory.Pistolets).ToList();
                    break;
                case "KoffieKoeken":
                    Recipes = Recipes.Where(x => x.Category == RecipeCategory.KoffieKoeken).ToList();
                    break;
                case "Koeken":
                    Recipes = Recipes.Where(x => x.Category == RecipeCategory.Koeken).ToList();
                    break;
            }

            Recipes.ForEach(x => x.UpdatePriceAtTime(_filterDate));
            this.recipes.ItemsSource = Recipes;
            this.recipes.Items.Refresh();
        }

        private void Recipes_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Recipe selected = e.Row.DataContext as Recipe;
                var result = dbContext.Recipes.SingleOrDefault(x => x.RecipeID == selected.RecipeID);

                if (result == null)
                {
                    try
                    {
                        result = new Recipe() { Name = "" , Category= RecipeCategory.TussenProduct};
                        dbContext.Recipes.Add(result);
                        dbContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                        UpdateTable();
                        return;
                    }
                }

                if (e.Column.Header.Equals("Naam"))
                {
                    var el = e.EditingElement as TextBox;
                    if (el.Text == null)
                        return;
                    result.Name = el.Text;
                    try { dbContext.SaveChanges(); } catch { }
                }

                if (e.Column.Header.Equals("Categorie"))
                {
                    var el = e.EditingElement as ComboBox;
                    if (el.SelectedValue == null)
                        return;
                    result.Category = (RecipeCategory)el.SelectedValue;
                    try { dbContext.SaveChanges(); } catch { }
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

        private void Recipes_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = (DataGrid)sender;
            var cell = grid.CurrentCell;

            if (cell.Column.Header.Equals("Prijs"))
            {
                Recipe recipe = (Recipe)cell.Item;
                var result = dbContext.Recipes.SingleOrDefault(x => x.RecipeID == recipe.RecipeID);
                dbContext.Dispose();
                RecipeView popup = new RecipeView(result.RecipeID);
                popup.ShowDialog();
                dbContext = new SqliteDbContext();
                UpdateTable();
            }

        }

        private void Close_MouseDown(object sender, MouseEventArgs e)
        {
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void Recipes_TextChanged(object sender, TextChangedEventArgs e)
        {
            _filterName = Filter.Text;
            UpdateTable();
        }

        private void Recipes_CategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.categories.SelectedItem != null)
            {
                this._filterCategory = this.categories.SelectedItem.ToString();
            }
            UpdateTable();
        }
        
        private void Recipes_DateChanged(object sender, SelectionChangedEventArgs e)
        {
            this._filterDate = (DateTime)this.datePicker.SelectedDate;
            UpdateTable();
        }
    }
}
