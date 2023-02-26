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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bakery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            using var db = new SqliteDbContext();
            InitializeComponent();

            //var tmp = new RecipeRecipe();
            //var tmp = db.RecipeRecipes;
            //db.RecipeRecipes.Remove(tmp);
            //db.SaveChanges();

            //db.Add(new Recipe() { Name = "brood", Category = RecipeCategory.Brood });
            //db.Add(new Recipe() { Name = "deeg", Category = RecipeCategory.TussenProduct });
            //db.Add(new Recipe() { Name = "deegbrood", Category = RecipeCategory.Brood });
            //db.Add(new Ingredient() { Unit = Unit.liter, Name = "water" });
            //db.Add(new Ingredient() { Unit = Unit.liter, Name = "melk" });
            //db.Add(new Ingredient() { Unit = Unit.kg, Name = "bloem" });
            //db.SaveChanges();
            //db.Add(new IngredientPrice() { IngredientID = 1, From = DateTime.Now, Price = 1 });
            //db.Add(new IngredientPrice() { IngredientID = 1, From = DateTime.Now.AddDays(-1), Price = 0.8 });
            //db.Add(new IngredientPrice() { IngredientID = 2, From = DateTime.Now.AddDays(-1), Price = 1 });
            //db.Add(new IngredientPrice() { IngredientID = 3, From = DateTime.Now.AddDays(-1), Price = 0.1 });
            //db.Add(new RecipeIngredient() { RecipeID = 1, IngredientID = 1, Amount = 5 });
            //db.Add(new RecipeIngredient() { RecipeID = 1, IngredientID = 2, Amount = 100 });
            //db.Add(new RecipeIngredient() { RecipeID = 2, IngredientID = 1, Amount = 1 });
            //db.Add(new RecipeIngredient() { RecipeID = 2, IngredientID = 2, Amount = 1 });
            //db.Add(new RecipeIngredient() { RecipeID = 3, IngredientID = 3, Amount = 15 });
            //db.Add(new RecipeRecipe() { FullRecipeID = 3, InBetweenRecipeID = 2, Amount= 10 });
            //db.SaveChanges();


            //db.Recipes.Include(x => x.InBetweenRecipes).ThenInclude(x => x.InBetweenRecipe);
            //db.Recipes.Include(x => x.Ingredients).ThenInclude(x => x.Ingredient).ThenInclude(x => x.Prices);
            //foreach (Recipe recipe in db.Recipes.Include(x => x.Ingredients).ThenInclude(x => x.Ingredient).ThenInclude(x => x.Prices))
            //{
            //    var tmp1 = recipe.GetPriceAtTime(DateTime.Now);
            //}



            //db.Ingredients.Include(x => x.Prices).Load<Ingredient>();
            //List<Ingredient> tmp = db.Ingredients.ToList();
            //this.ingredients.ItemsSource = tmp;


            //this.ingredients.CellEditEnding += ingredients_CellEditEnding;

            //void ingredients_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
            //{
            //    if (e.EditAction == DataGridEditAction.Commit)
            //    {
            //        var column = e.Column as DataGridBoundColumn;
            //        if (column != null)
            //        {
            //            var bindingPath = (column.Binding as Binding).Path.Path;
            //            int rowIndex = e.Row.GetIndex();
            //            var result = db.Ingredients[rowIndex];

            //            Ingredient update = tmp[rowIndex];
            //            if (bindingPath == "Name")
            //            {
            //                var el = e.EditingElement as TextBox;
            //                update.Name = el.Text;
            //            }

            //            var result = db.Ingredients[];
            //            if (result != null)
            //            {
            //                result.SomeValue = "Some new value";
            //                db.SaveChanges();
            //            }

            //            db.SaveChanges();
            //        }
            //    }
            //}
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
