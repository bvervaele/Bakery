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
using Bakery.Data;
using Bakery.Models;
using Microsoft.EntityFrameworkCore;

namespace Bakery.MVVM.View
{
    /// <summary>
    /// Interaction logic for PricesView.xaml
    /// </summary>
    public partial class PricesView : Window
    {
        public List<IngredientPrice> Prices { get; set; }

        public int IngredientId { get; set; }

        public SqliteDbContext dbContext { get; set; }

        public PricesView(int id)
        {
            InitializeComponent();
            dbContext = new SqliteDbContext();
            IngredientId = id;
            UpdateTable();

            this.prices.CellEditEnding += Prices_CellEditEnding;
            this.prices.PreviewKeyDown += Prices_DeletingItem;
            this.prices.AddingNewItem += Prices_AddRow;
        }

        public void UpdateTable()
        {
            dbContext.Ingredients.Include(x => x.Prices).Load<Ingredient>();
            Prices = dbContext.Ingredients.SingleOrDefault(x => x.IngredientID == IngredientId).Prices.OrderByDescending(x => x.From).ToList();
            this.prices.ItemsSource = Prices;
            this.prices.Items.Refresh();
        }

        private void Prices_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                IngredientPrice selected = e.Row.DataContext as IngredientPrice;
                var result = dbContext.IngredientPrices.SingleOrDefault(x => x.IngredientID == IngredientId && x.From == selected.From);

                if (result == null)
                {
                    return;
                    //try
                    //{
                    //    result = new IngredientPrice() { IngredientID = IngredientId };
                    //    dbContext.IngredientPrices.Add(result);
                    //    dbContext.SaveChanges();
                    //}
                    //catch (Exception)
                    //{
                    //    UpdateTable();
                    //    return;
                    //}
                }

                if (e.Column.Header.Equals("Datum"))
                {
                    try
                    {
                        var el = e.EditingElement as TextBox;
                        if (el.Text == null)
                            return;
                        DateTime newDate = DateTime.ParseExact(el.Text, "dd/MM/yyyy", null);
                        if (Prices.Where(x => x.From == newDate).Any())
                        {
                            UpdateTable();
                            return;
                        }

                        var newPrice = new IngredientPrice() { IngredientID = IngredientId, Price = result.Price };
                        dbContext.IngredientPrices.Remove(result);
                        try { dbContext.SaveChanges(); } catch { }

                        newPrice.From = newDate;

                        dbContext.IngredientPrices.Add(newPrice);
                        try { dbContext.SaveChanges(); } catch { }
                    } catch { }
                }

                if (e.Column.Header.Equals("Prijs"))
                {
                    var el = e.EditingElement as TextBox;
                    if (el.Text == null)
                        return;
                    result.Price = double.Parse(el.Text.Replace('.', ','));
                    dbContext.SaveChanges();
                }

                UpdateTable();
            }
        }

        private void Prices_DeletingItem(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var grid = (DataGrid)sender;
                foreach (var row in grid.SelectedItems)
                {
                    try
                    {
                        IngredientPrice ingredientPrice = row as IngredientPrice;
                        dbContext.IngredientPrices.Remove(ingredientPrice);
                        dbContext.SaveChanges();
                    }
                    catch (Exception) { }
                }
                UpdateTable();
            }
        }

        private void Prices_AddRow(object sender, AddingNewItemEventArgs e)
        {
            try
            {
                IngredientPrice price = new IngredientPrice() { IngredientID = IngredientId };
                int days = 0;
                while (true)
                {
                    if (!dbContext.IngredientPrices.Where(x => x.IngredientID == this.IngredientId && x.From.Date == DateTime.Now.AddDays(days).Date).Any())
                        break;
                    days--;
                }
                price.From = DateTime.Now.AddDays(days);
                dbContext.IngredientPrices.Add(price);
                dbContext.SaveChanges();
                e.NewItem = price;
            }catch (Exception) {}
        }
    }
}
