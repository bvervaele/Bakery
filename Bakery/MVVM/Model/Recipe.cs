using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Models
{
    public class Recipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecipeID { get; set; }
        public string Name { get; set; }
        public RecipeCategory Category { get; set; }

        public List<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
        public List<RecipeRecipe> InBetweenRecipes { get; set; } = new List<RecipeRecipe>();
        public List<RecipeRecipe> PartOfRecipes { get; set; } = new List<RecipeRecipe>();

        [NotMapped]
        public double Price { get; set; } = 0;

        public void UpdatePriceAtTime(DateTime dateTime)
        {
            double totalPrice = 0;
            double totalAmount= 0;
            if (Ingredients.Any())
            {
                totalPrice += Ingredients.Sum(x => x.GetPriceAtTime(dateTime));
                totalAmount += Ingredients.Sum(x => x.Amount);
            }

            if (InBetweenRecipes.Any())
            {
                totalPrice += InBetweenRecipes.Sum(x => x.GetPriceAtTime(dateTime));
                totalAmount += InBetweenRecipes.Sum(x => x.Amount);
            }

            if (totalAmount == 0)
                Price = 0;
            else if (Category == RecipeCategory.TussenProduct)
                Price = totalPrice / totalAmount;
            else
                Price = totalPrice;
        }
    }

    public enum RecipeCategory
    {
        TussenProduct,
        Taart,
        DroogGebak,
        Brood,
        Pistolets,
        KoffieKoeken,
        Koeken,
    }
}
