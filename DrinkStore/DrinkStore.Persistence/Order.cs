using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkStore.Persistence
{
    public class Order
    {
        //Azonosító
        [Key]
        public Int32 Id { get; set; }

        //Név
        public String Name { get; set; }

        //Cím
        public String Address { get; set; }

        //Email cím
        public String Email { get; set; }

        //Telefonszám
        public String PhoneNumber{ get; set; }

        //Teljesítettség
        public Boolean IsDone { get; set; }

        //Dátum
        public DateTime Date { get; set; }

    }
}
