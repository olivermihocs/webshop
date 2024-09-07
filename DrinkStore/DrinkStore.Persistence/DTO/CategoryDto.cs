using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DrinkStore.Persistence.DTO
{
    public class CategoryDto
    {
        [Key]
        public Int32 Id { get; set; }

        //Név
        public String Name { get; set; }

        //FőKategória
        [Required]
        public Int32 MainCategoryId { get; set; }

        public static explicit operator Category(CategoryDto dto) => new Category
        {
            Id = dto.Id,
            Name = dto.Name,
            MainCategoryId = dto.MainCategoryId,
        };

        public static explicit operator CategoryDto(Category c) => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            MainCategoryId = c.MainCategoryId,
        };
    }
}
