using Bakery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.MVVM.ViewModel
{
    public class IngredientViewModel
    {
        public List<Units> AllUnits { get; } = new List<Units>() { Units.liter, Units.gram, Units.stuk, Units.doos, Units.blik };
    }
}
