using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DrinkStore.Persistence.DTO
{
    public class OrderDto
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
        public String PhoneNumber { get; set; }

        //Teljesítettség
        public Boolean IsDone { get; set; }

        public DateTime Date { get; set; }


        public static explicit operator Order(OrderDto o) => new Order
        {
            Id = o.Id,
            Name = o.Name,
            Address = o.Address,
            Email = o.Email,
            PhoneNumber = o.PhoneNumber,
            IsDone = o.IsDone,
            Date = o.Date
        };

        public static explicit operator OrderDto(Order o) => new OrderDto
        {
            Id = o.Id,
            Name = o.Name,
            Address = o.Address,
            Email = o.Email,
            PhoneNumber = o.PhoneNumber,
            IsDone = o.IsDone,
            Date =  o.Date
        };
    }
}
