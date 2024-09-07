using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkStore.Persistence;

namespace DrinkStore.Web.Models
{
    public class HomeViewModel
    {
        //Főkategóriák
        public IEnumerable<IEnumerable<Category>> Categories { get; set; }
    }
}
