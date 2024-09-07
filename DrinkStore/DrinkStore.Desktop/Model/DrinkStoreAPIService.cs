using DrinkStore.Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DrinkStore.Desktop.Model
{
    public class DrinkStoreAPIService
    {
        private readonly HttpClient _client;

        public Boolean IsUserLoggedIn { get; private set; } //Be van e jelentkezve a felhasználó

        public DrinkStoreAPIService(string baseAddress)
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        //Be- és kijelentkezés
        #region Authentication

        //Bejelentkezés
        public async Task<bool> LoginAsync(string name, string password)
        {
            LoginDto user = new LoginDto
            {
                UserName = name,
                Password = password
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Account/Login", user);

            if (response.IsSuccessStatusCode)
            {
                IsUserLoggedIn = true;
                return true;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        //Kijelentkezés
        public async Task LogoutAsync()
        {
            HttpResponseMessage response = await _client.PostAsync("api/Account/Logout", null);

            if (response.IsSuccessStatusCode)
            {
                IsUserLoggedIn = false;
                return;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        #endregion

        //Termék
        #region Product

        //Termék frissítése
        public async Task UpdateProductAsync(Int32 Id, ProductDto product)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/Products/{Id}",product);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }

        //Új termék
        public async Task CreateProductAsync(ProductDto product)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Products/", product);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }

        //Termék lekérdezése ID alapján
        public async Task<ProductDto> LoadProductByIdAsync(Int32 Id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/Products/{Id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<ProductDto>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        //Kategóriához tartozó termékek
        public async Task<IEnumerable<ProductDto>> LoadProductsByCategoryAsync(Int32 Id)
        {
            HttpResponseMessage response = await _client.GetAsync("api/Categories/" + Id.ToString());

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<ProductDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }
        #endregion

        //Kategóriák és Főkategóriák 
        #region Categories

        //Főkategóriák
        public async Task<IEnumerable<MainCategoryDto>> LoadMainCategoriesAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/MainCategories");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<MainCategoryDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        //Főkategóriához tartozó kategóriák
        public async Task<IEnumerable<CategoryDto>> LoadCategoriesByMainCategoryAsync(Int32 Id)
        {
            HttpResponseMessage response = await _client.GetAsync("api/MainCategories/"+Id.ToString());

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<CategoryDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }
        #endregion

        //Rendelések
        #region Orders

        //Rendelések
        public async Task<IEnumerable<OrderDto>> LoadOrdersAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/Orders");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<OrderDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        //Rendeléshez tartozó sorok
        public async Task<IEnumerable<OrderLineDto>> LoadOrderLinesByOrderAsync(Int32 Id)
        {
            HttpResponseMessage response = await _client.GetAsync("api/Orders/" + Id.ToString());

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<OrderLineDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        //Szűrt rendelések
        public async Task<IEnumerable<OrderDto>> LoadFilteredOrdersAsync(FilterDto filter)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Orders/", filter);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<OrderDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        //Rendelés frissítése
        public async Task<Boolean> UpdateOrderAsync(Int32 Id, OrderDto orderDto)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/Orders/{Id}", orderDto);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }
        #endregion
    }
}
