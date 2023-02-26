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
    /// Interaction logic for IngredientView.xaml
    /// </summary>
    public partial class IngredientView : UserControl
    {
        public List<Ingredient> Ingredients { get; set; }

        public SqliteDbContext dbContext { get; set; }

        public string _filterName = "";

        public DateTime _filterDate = DateTime.Now;

        public IngredientView()
        {
            dbContext = new SqliteDbContext();

            InitializeComponent();
            UpdateTable();

            this.ingredients.CellEditEnding += Ingredients_CellEditEnding;
            this.ingredients.PreviewKeyDown += Ingredients_DeletingItem;
            this.ingredients.MouseDoubleClick += Ingredients_DoubleClick;
        }

        public void UpdateTable()
        {
            if (this.ingredients == null)
                return;

            dbContext.Ingredients.Include(x => x.Prices).Load<Ingredient>();
            Ingredients = dbContext.Ingredients.OrderBy(x => x.Name).ToList();
            if(_filterName != "")
                Ingredients = Ingredients.Where(x => x.Name.StartsWith(_filterName)).ToList();

            Ingredients.ForEach(x => x.UpdatePriceAtTime(_filterDate));
            this.ingredients.ItemsSource = Ingredients;
            this.ingredients.Items.Refresh();
        }

        private void Ingredients_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Ingredient selected = e.Row.DataContext as Ingredient;
                var result = dbContext.Ingredients.SingleOrDefault(x => x.IngredientID == selected.IngredientID);

                if(result == null)
                {
                    try
                    {
                        result = new Ingredient() { Name = "" };
                        dbContext.Ingredients.Add(result);
                        dbContext.SaveChanges();
                    }
                    catch (Exception) {
                        UpdateTable();
                        return;
                    }
                }

                if (e.Column.Header.Equals("Naam"))
                {
                    var el = e.EditingElement as TextBox;
                    result.Name = el.Text;
                    try { dbContext.SaveChanges(); } catch { }
                }

                if (e.Column.Header.Equals("Eenheid"))
                {
                    var el = e.EditingElement as ComboBox;
                    result.Unit = (Units) el.SelectedValue;
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
                    }catch (Exception) { }
                }
                UpdateTable();
            }
        }

        private void Ingredients_DoubleClick(object sender, MouseButtonEventArgs e )
        {
            var grid = (DataGrid)sender;
            var cell = grid.CurrentCell;

            if (cell.Column.Header.Equals("Prijs"))
            {
                Ingredient ingredient = (Ingredient)cell.Item;
                var result = dbContext.Ingredients.SingleOrDefault(x => x.IngredientID == ingredient.IngredientID);
                dbContext.Dispose();
                PricesView popup = new PricesView(result.IngredientID);
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

        private void Ingredients_TextChanged(object sender, TextChangedEventArgs e)
        {
            _filterName = Filter.Text;
            UpdateTable();
        }

        private void Ingredients_DateChanged(object sender, SelectionChangedEventArgs e)
        {
            this._filterDate = (DateTime)this.datePicker.SelectedDate;
            UpdateTable();
        }
    }
}
