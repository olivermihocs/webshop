using System;

namespace DrinkStore.Desktop.Model
{

    //Kategória megjelenítéséhez szükséges egyéb adatok átadására szolgál
    public class CategoryEventArgs : EventArgs
    {
        public Int32 CategoryId { get; set; }

        public String CategoryName { get; set; }

    }
}
