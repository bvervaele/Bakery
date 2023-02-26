using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Bakery.Models
{
    public class Ingredient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IngredientID { get; set; }
        public string Name { get; set; }
        public Units Unit { get; set; }

        public List<RecipeIngredient> Recipes { get; set; }
        public List<IngredientPrice> Prices { get; set; }

        [NotMapped]
        public double Price { get; set; } = 0;

        public void UpdatePriceAtTime(DateTime dateTime)
        {
            if (Prices == null)
                return;

            var prices = Prices.OrderByDescending(x => x.From).Where(x => x.From <= dateTime).ToList();
            if (prices.Any())
                Price=  prices.First().Price;
            
        }

    }

    public enum Units
    {
        liter,
        gram,
        stuk,
        doos,
        blik
    }
}
