using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Models
{
    public class IngredientPrice
    {
        public int IngredientID { get; set; }
        public double Price { get; set; }
        public DateTime From { get; set; }

        public Ingredient Ingredient { get; set; }
    }
}
