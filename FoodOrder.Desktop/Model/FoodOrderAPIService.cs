using FoodOrder.DTO;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Desktop.Model
{
    public class FoodOrderAPIService : IDisposable
    {
        private readonly HttpClient _client;

        public FoodOrderAPIService(string baseAddress)
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        #region Authentication

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
                return true;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return false;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task LogoutAsync()
        {
            HttpResponseMessage response = await _client.PostAsync("api/Account/Logout", null);

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            throw new NetworkException("Service returned response: " + response);
        }

        #endregion

        #region Order

        public async Task<IEnumerable<OrderDto>> LoadOrdersAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/Orders/");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<OrderDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<IEnumerable<OrderDto>> LoadSearchedOrdersAsync(int searchNumber, string searchBar)
        {
            HttpResponseMessage response;
            if (searchNumber == 1)
            {
                response = await _client.GetAsync("api/Orders/getdoneorders");
            }
            else if(searchNumber == 2)
            {
                response = await _client.GetAsync("api/Orders/getundoneorders");
            }
            else if(searchNumber == 3)
            {
                if(searchBar == "" || searchBar == null)
                {
                    response = await _client.GetAsync("api/Orders/");
                }
                else
                {
                    response = await _client.GetAsync($"api/Orders/getordersbyname/{searchBar}");
                }
            }
            else if (searchNumber == 4)
            {
                if (searchBar == "" || searchBar == null)
                {
                    response = await _client.GetAsync("api/Orders/");
                }
                else
                {
                    response = await _client.GetAsync($"api/Orders/getordersbyaddress/{searchBar}");
                }
            }
            else
            {
                response = await _client.GetAsync("api/Orders/");
            }

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<OrderDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task UpdateOrderAsync(OrderDto order)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/Orders/{order.OrderId}", order);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response " + response);
            }
        }

        #endregion

        #region Product

        public async Task<IEnumerable<ProductDto>> LoadProductsAsync(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/Orders/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<ProductDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<IEnumerable<ProductDto>> LoadAllProductsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync($"api/Products/");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<ProductDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task CreateProductAsync(ProductDto product)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Products/", product);
            product.Id = (await response.Content.ReadAsAsync<ProductDto>()).Id;

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }

        #endregion

        #region Category

        public async Task<bool> CategoryIsDrink(string name)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/Products/Isdrink/{name}").ConfigureAwait(true);

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }

            return (await response.Content.ReadAsAsync<bool>().ConfigureAwait(true));
        }

        #endregion


        public void Dispose()
        {
            //((IDisposable)_client).Dispose();
        }
    }
}
