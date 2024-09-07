using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkStore.Persistence
{
    public class Employee : IdentityUser<int>
    {
        //Név
        public String FullName { get; set; }

    }
}
