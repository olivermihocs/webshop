using DrinkStore.Desktop.Model;
using DrinkStore.Persistence;
using DrinkStore.Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DrinkStore.Desktop.ViewModel
{
    public class NewProductViewModel : ViewModelBase
    {
        private readonly DrinkStoreAPIService _service;

        private ProductDto _productDto;

        public event EventHandler EndNewProduct;

        public ProductDto ProductDto
        {
            get => _productDto;
            set
            {
                _productDto = value;
                _productDto.PriceVAT = (int)(_productDto.Price * 1.27);
                OnPropertyChanged();
            }
        }

        public DelegateCommand SaveNewProductCommand { get; private set; }
        public DelegateCommand CancelProductCommand { get; private set; }

        public NewProductViewModel(DrinkStoreAPIService service, Int32 CategoryId)
        {
            _service = service;
            _productDto = new ProductDto()
            {
                CategoryId = CategoryId,
                Packagings = PackagingDto.Convert(Packaging.Piece),
                PriceVAT = 0
            };

            SaveNewProductCommand = new DelegateCommand(_ => SaveNewProduct());
            CancelProductCommand = new DelegateCommand(_ => OnEndNewProduct());
        }

        private async void SaveNewProduct()
        {
            List<string> errorMsg = new List<string>();
            if (ProductDto.TypeNo == null || ProductDto.TypeNo == "")
            {
                errorMsg.Add("A tipusszámot kötelező megadni");
            }
            if (ProductDto.Manufacturer == null || ProductDto.Description == "")
            {
                errorMsg.Add("A gyártót kötelező megadni.");
            }
            if (ProductDto.Description == null || ProductDto.Description == "")
            {
                errorMsg.Add("A leírást kötelező megadni.");
            }
            if (ProductDto.Price <= 0)
            {
                errorMsg.Add("Az árnak pozitív egész számnak kell lennie.");
            }
            if (ProductDto.Stock <= 0)
            {
                errorMsg.Add("A készletnek pozitív egész számnak kell lennie.");
            }
            if (PackagingDto.Convert(ProductDto.Packagings)==0)
            {
                errorMsg.Add("Legalább egy elérhető kiszerelést meg kell megjelölni.");
            }
            if (errorMsg.Count > 0)
            {
                OnMessageApplication(String.Join("\n", errorMsg.ToArray()));
                return;
            }
            try
            {
                await _service.CreateProductAsync(ProductDto);
                OnEndNewProduct();
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Hiba történt! ({ex.Message})");
            }
        }

        private void OnEndNewProduct()
        {
            EndNewProduct?.Invoke(this, EventArgs.Empty);
        }

    }

}
