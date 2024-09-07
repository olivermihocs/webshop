using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkStore.Persistence
{
    //Alkategória
    public class Category
    {

        [Key]
        public Int32 Id { get; set; }

        //FőKategória
        [Required]
        public Int32 MainCategoryId { get; set; }
        public virtual MainCategory MainCategory { get; set; }

        //Név
        public String Name { get; set; }

        //Termékek listája
        public ICollection<Product> Products { get; set; }


    }
}
