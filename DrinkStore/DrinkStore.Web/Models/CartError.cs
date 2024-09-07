using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkStore.Web.Models
{
    //Kosárműveleteknél előforduló lehetséges hibákhoz használt enumerátor
    public enum CartError
    {
        None,
        PackagingInvalid,
        QuantityInvalid,
        ProductInvalid,
    }
}
