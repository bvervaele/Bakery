using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Models
{
    public class RecipeIngredient
    {
        public int RecipeID { get; set; }
        public int IngredientID { get; set; }
        public double Amount { get; set; }

        public Recipe Recipe { get; set; }
        public Ingredient Ingredient { get; set; }

        [NotMapped]
        public string Unit
        {
            get
            {
                if (Ingredient == null)
                    return "";
                return Ingredient.Unit.ToString();
            }
            set { }
        }

        [NotMapped]
        public string Name { 
            get { 
                if(Ingredient == null)
                    return "";
                return Ingredient.Name; 
            } 
            set { } 
        }

        public double GetPriceAtTime(DateTime dateTime)
        {
            Ingredient.UpdatePriceAtTime(dateTime);
            return Ingredient.Price * Amount;
        }
    }
}
