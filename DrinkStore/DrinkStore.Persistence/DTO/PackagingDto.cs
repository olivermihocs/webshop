using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkStore.Persistence.DTO
{
    public class PackagingDto
    {
        public Int32 Id { get; set; }

        public Boolean IsAvailable { get; set; }

        //Szám->tömb
        public static PackagingDto[] Convert(Packaging packagings)
        {
            List<PackagingDto> result = new List<PackagingDto>();

            Int32 pId = 0;
            foreach (Packaging p in Enum.GetValues(typeof(Packaging)))
            {
                if (p > 0)
                {
                    result.Add(new PackagingDto
                    {
                        Id = pId++,
                        IsAvailable = packagings.HasFlag(p)
                    });
                }
            }

            return result.ToArray();
        }

        //Tömb->szám
        public static Packaging Convert(PackagingDto[] packagings)
        {
            if (packagings == null || packagings.Length == 0)
                return Packaging.None;

            Packaging result = Packaging.None;
            foreach (var p in packagings)
            {
                if (p.IsAvailable)
                {
                    result += (1 << p.Id);
                }
            }

            return result;
        }

        public static int GetValueOfPackaging(Packaging packaging)
        {
            switch (packaging)
            {
                case Packaging.Piece: //Darab
                    return 1;
                case Packaging.Shrink: //Zsugorfólia
                    return 6;
                case Packaging.Bin: //Rekesz
                    return 12;
                case Packaging Tray: //Tálca
                    return 24;
            }
        }
    }
}

