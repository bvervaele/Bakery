using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bakery.Core;
using Bakery.Data;

namespace Bakery.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand IngredientViewCommand { get; set; }
        public RelayCommand RecipiesViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; } 
        public IngredientViewModel IngredientVM { get; set; }
        public RecipesViewModel RecipiesVM { get; set; }

        private object _currentView;

        public SqliteDbContext dbContext { get; set; }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            dbContext = new SqliteDbContext();  
            HomeVM = new HomeViewModel();
            IngredientVM = new IngredientViewModel();
            RecipiesVM = new RecipesViewModel();

            CurrentView = RecipiesVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });
            IngredientViewCommand = new RelayCommand(o =>
            {
                CurrentView = IngredientVM;
            });
            RecipiesViewCommand = new RelayCommand(o =>
            {
                CurrentView = RecipiesVM;
            });
        }
    }
}
