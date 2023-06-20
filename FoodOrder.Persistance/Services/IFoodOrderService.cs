namespace FoodOrder.Persistance.Services
{
    public interface IFoodOrderService
    {
        public IList<Category> GetAllCategories();

        public IList<Products> GetTop10Products();

        public IList<Products> GetAllProducts(string categoryName);

        public IList<Products> GetAllProductsForOrder(int orderId);

        public Products GetProduct(int id);

        public int GetCurrentOrderId();

        public bool SaveOrder(Orders order);

        public bool UpdateOrder(Orders order);

        public IList<Orders> GetOrders();
        public IList<Orders> LoadDoneOrdersAsync();
        public IList<Orders> LoadUnDoneOrdersAsync();
        public IList<Orders> LoadOrdersByNameAsync(string name);
        public IList<Orders> LoadOrdersByAddressAsync(string address);


        public Orders GetOrder(int id);

        public Task<Products>? CreateProduct(Products product);
        public bool UpdateProducts(Products product);

        public IList<Products> GetAllProducts();

        public bool CategoryIsDrink(string name);

    }
}
