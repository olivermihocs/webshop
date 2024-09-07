using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkStore.Persistence.DTO
{
    public class OrderLineDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int TotalQuantity { get; set; }

        public PackagingDto[] Packagings { get; set; }

        public ProductDto Product { get; set; }

        public static explicit operator OrderLine(OrderLineDto dto) => new OrderLine
        {
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            Packaging = PackagingDto.Convert(dto.Packagings)
        };

        public static explicit operator OrderLineDto(OrderLine l) => new OrderLineDto
        {
            OrderId = l.OrderId,
            ProductId = l.ProductId,
            Quantity = l.Quantity,
            TotalQuantity = l.Quantity*PackagingDto.GetValueOfPackaging(l.Packaging),
            Packagings = PackagingDto.Convert(l.Packaging),
        };
    }
}
