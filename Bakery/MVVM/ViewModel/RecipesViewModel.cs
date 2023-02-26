using Bakery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    public class RecipesViewModel
    {
        public List<RecipeCategory> AllRecipyCategories { get; } = new List<RecipeCategory>() { RecipeCategory.TussenProduct, RecipeCategory.Taart, RecipeCategory.DroogGebak, RecipeCategory.Brood, RecipeCategory.Pistolets, RecipeCategory.KoffieKoeken, RecipeCategory.Koeken,};
    }
}
