using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DrinkStore.Persistence;

namespace DrinkStore.Web.Models
{
    public class CartViewModel
    {

        //Nézetmodell a kosár megjelenítéséhez
        public IEnumerable<Tuple<Product, int, int, String>> CartProducts { get; set; }
        
        public Int32 Price { get; set; }

        public Int32 VAT { get; set; }

        //A rendeléshez szükséges adatokat tároló változók

        [DisplayName("Név")]
        [Required(ErrorMessage = "A név megadása kötelező.")]
        public string Name { get; set; }

        [DisplayName("Cím")]
        [Required(ErrorMessage = "A cím megadása kötelező.")]
        public string Address { get; set; }
        
        [DisplayName("Telefonszám")]
        [Required(ErrorMessage = "A telefonszám megadása kötelező.")]
        [Phone(ErrorMessage = "A telefonszám formátuma nem megfelelő.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "A telefonszám formátuma nem megfelelő.")]
        public string PhoneNumber { get; set; }

        [DisplayName("Email cím")]
        [Required(ErrorMessage = "E-mail cím megadása kötelező.")]
        [EmailAddress(ErrorMessage = "Az e-mail cím nem megfelelő formátumú.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
