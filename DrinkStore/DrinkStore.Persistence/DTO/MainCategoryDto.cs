using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DrinkStore.Persistence.DTO
{
    public class MainCategoryDto
    {
        [Key]
        public Int32 Id { get; set; }

        //Név
        public String Name { get; set; }

        public static explicit operator MainCategory(MainCategoryDto dto) => new MainCategory
        {
            Id = dto.Id,
            Name = dto.Name
        };

        public static explicit operator MainCategoryDto(MainCategory c) => new MainCategoryDto
        {
            Id = c.Id,
            Name = c.Name
        };
    }
}
