using DrinkStore.Desktop.Model;
using DrinkStore.Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace DrinkStore.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly DrinkStoreAPIService _service;

        private ObservableCollection<MainCategoryDto> _mainCategories;
        private ObservableCollection<CategoryDto> _categories;
        private ObservableCollection<ProductDto> _products;

        private MainCategoryDto _selectedMainCategory;
        private CategoryDto _selectedCategory;
        private ProductDto _selectedProduct;

        private Int32 _updateStockValue;

        public ObservableCollection<MainCategoryDto> MainCategories
        {
            get => _mainCategories;
            set
            {
                _mainCategories = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<CategoryDto> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ProductDto> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public ProductDto SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
                if (SelectedProduct != null)
                {
                    UpdateStockValue = SelectedProduct.Stock;
                }
                else
                {
                    UpdateStockValue = 0;
                }
            }
        }

        public CategoryDto SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        public MainCategoryDto SelectedMainCategory
        {
            get => _selectedMainCategory;
            set
            {
                _selectedMainCategory = value;
                OnPropertyChanged();
            }
        }

        public Int32 UpdateStockValue
        {
            get => _updateStockValue;
            set
            {
                _updateStockValue = value;
                OnPropertyChanged();
            }
        }


        //Frissítés
        public DelegateCommand RefreshCommand { get; private set; }

        //Főkategória kiválasztása
        public DelegateCommand SelectMainCategoryCommand { get; private set; }

        //Kategória kiválasztása
        public DelegateCommand SelectCategoryCommand { get; private set; }

        //Rendelések nézet megnyitása
        public DelegateCommand OpenOrdersCommand { get; private set; }

        //Új termék hozzáadása
        public DelegateCommand CreateNewProductCommand { get; private set; }

        //Termék frissítése
        public DelegateCommand UpdateProductStockCommand { get; private set; }

        //Kijelentkezés
        public DelegateCommand LogOutCommand { get; private set; }

        //Kilépés
        public DelegateCommand ExitCommand { get; set; }

        public event EventHandler ExitApplication;
        public event EventHandler<CategoryEventArgs> CreateNewProductStarted;
        public event EventHandler OpenOrders;
        public event EventHandler LogOut;
        

        public MainViewModel(DrinkStoreAPIService service)
        {
            _service = service;

            RefreshCommand = new DelegateCommand(_ => LoadMainCategoriesAsync());
            SelectMainCategoryCommand = new DelegateCommand(_ => LoadSelectedMainCategoryAsync());
            SelectCategoryCommand = new DelegateCommand(_ => LoadSelectedCategoryAsync());
            OpenOrdersCommand = new DelegateCommand(_ => OnOpenOrders());
            CreateNewProductCommand = new DelegateCommand(_ => OnCreateNewProductStarted());
            UpdateProductStockCommand = new DelegateCommand(param => UpdateProductStock(param as String));
            ExitCommand = new DelegateCommand(_ => OnExitApplication());
            LogOutCommand = new DelegateCommand(_ => OnLogOut());
        }
        private void OnExitApplication()
        {
            ExitApplication?.Invoke(this, EventArgs.Empty);
        }

        private void OnLogOut()
        {
            LogOut?.Invoke(this, EventArgs.Empty);
        }

        private void OnOpenOrders()
        {
            OpenOrders?.Invoke(this, EventArgs.Empty);
        }

        public void OnReload(object sender, EventArgs e)
        {
            LoadMainCategoriesAsync();
        }

        private async void UpdateProductStock(String text)
        {
            if (SelectedProduct == null)
            {
                OnMessageApplication("Válasszon ki egy terméket előszőr.");
                return;
            }
            try
            {
                int value = int.Parse(text);
                if (value < 0) {
                    OnMessageApplication("Kérem természetes számot adjon meg.");
                    return;
                }
                if (value != SelectedProduct.Stock)
                {
                    SelectedProduct.Stock = value;
                    await _service.UpdateProductAsync(SelectedProduct.Id, SelectedProduct);
                    OnMessageApplication("Termék(ID:"+SelectedProduct.Id+") frissítve, új készlet: "+value+"db");
                    LoadSelectedCategoryAsync();
                }
            }
            catch
            {
                OnMessageApplication("Kérem számot adjon meg.");
            }
        }

        private void OnCreateNewProductStarted()
        {
            if (SelectedCategory != null)
            {
                CreateNewProductStarted(this, new CategoryEventArgs { CategoryId = SelectedCategory.Id, CategoryName = SelectedCategory.Name });
            }
            else
            {
                OnMessageApplication("Válasszon ki egy kategóriát, amelyhez a terméket hozzá szeretné adni.");
            }
        }
        public void OnCreateNewProductEnded(object sender, EventArgs e)
        {
            LoadSelectedCategoryAsync();
        }

        private async void LoadMainCategoriesAsync()
        {
            Categories = null;
            Products = null;
            try
            {
                MainCategories = new ObservableCollection<MainCategoryDto>(await _service.LoadMainCategoriesAsync());
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Hiba történt! ({ex.Message})");
            }
        }

        private async void LoadSelectedMainCategoryAsync()
        {
            Products = null;
            try
            {
                if (SelectedMainCategory != null){
                    Categories = new ObservableCollection<CategoryDto>(await _service.LoadCategoriesByMainCategoryAsync(SelectedMainCategory.Id));
                }
                
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Hiba történt! ({ex.Message})");
            }
        }

        private async void LoadSelectedCategoryAsync()
        {
            try
            {
                if (SelectedCategory != null)
                {
                    Products = new ObservableCollection<ProductDto>(await _service.LoadProductsByCategoryAsync(SelectedCategory.Id));
                }

            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Hiba történt! ({ex.Message})");
            }
        }

        
    }
}
