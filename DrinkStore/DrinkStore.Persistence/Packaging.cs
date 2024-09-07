using System;

namespace DrinkStore.Persistence
{
    //Kiszerelés
    [Flags]
    public enum Packaging
    {
        None = 0,
        Piece = 1,//Darab
        Shrink = 2, //Zsugorfólia
        Tray = 4, //Tálca
        Bin = 8, //Rekesz
    }
}
