using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkStore.Persistence
{
    public class MainCategory
    {
        //Azonosító
        [Key]
        public Int32 Id { get; set; }

        //Név
        public String Name { get; set; }
    }
}
