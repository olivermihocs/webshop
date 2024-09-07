using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DrinkStore.Persistence.DTO
{
    public class ProductDto
    {
        //Modellszám
        [Key]
        public Int32 Id { get; set; }

        //Gyártó
        public String Manufacturer { get; set; }

        //Leírás
        public String Description { get; set; }

        public String TypeNo { get; set; }

        //Kategória
        [Required]
        public Int32 CategoryId { get; set; }

        //Nettó ár
        public Int32 Price { get; set; }

        //Bruttó ár
        public Int32 PriceVAT { get; set; }

        //Készlet
        public Int32 Stock { get; set; }

        //Csomagolás
        public PackagingDto[] Packagings { get; set; }


        public static explicit operator Product(ProductDto dto) => new Product
        {
            Id = dto.Id,
            Manufacturer = dto.Manufacturer,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            Price = dto.Price,
            Stock = dto.Stock,
            TypeNo = dto.TypeNo,
            Packaging = PackagingDto.Convert(dto.Packagings)
        };

        public static explicit operator ProductDto(Product p) => new ProductDto
        {
            Id = p.Id,
            Manufacturer = p.Manufacturer,
            Description = p.Description,
            CategoryId = p.CategoryId,
            Price = p.Price,
            Stock = p.Stock,
            TypeNo = p.TypeNo,
            Packagings = PackagingDto.Convert(p.Packaging),
            PriceVAT = (int)(p.Price*1.27)
        };
    }

    
}
