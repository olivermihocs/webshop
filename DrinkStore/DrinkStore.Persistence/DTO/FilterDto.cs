using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkStore.Persistence.DTO
{
    public class FilterDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public String Name { get; set; }
        public Boolean Done { get; set; }
        public Boolean NotDone { get; set; }
    }
}
