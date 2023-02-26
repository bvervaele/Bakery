using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Models
{
    public class RecipeRecipe
    {
        public int InBetweenRecipeID { get; set; }
        public int FullRecipeID { get; set; }
        public double Amount { get; set; }

        public Recipe InBetweenRecipe { get; set; }
        public Recipe FullRecipe { get; set; }

        [NotMapped]
        public string Name
        {
            get
            {
                if (InBetweenRecipe == null)
                    return "";
                return InBetweenRecipe.Name;
            }
            set { }
        }

        public double GetPriceAtTime(DateTime dateTime)
        {
            InBetweenRecipe.UpdatePriceAtTime(dateTime);
            return InBetweenRecipe.Price * Amount;
        }
    }
}
