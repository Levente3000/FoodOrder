using FoodOrder.Desktop.Model;
using FoodOrder.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Xml.Linq;

namespace FoodOrder.Desktop.ViewModel
{
    public class SelectedListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OrderViewModel)
                return value;
            return null!;
        }
    }

    public class CategoryNameConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            // ellenőrizzük az értéket
            if (value == null || !(value is string))
                return Binding.DoNothing; // ha nem megfelelő, nem csinálunk semmit

            // ellenőrizzük a paramétert
            if (parameter == null || !(parameter is IEnumerable<String>))
                return Binding.DoNothing;

            List<String> categoryNames = (parameter as IEnumerable<String>)!.ToList();
            Int32 index = categoryNames.IndexOf(value.ToString()!);

            if (index < 0 || index >= categoryNames.Count)
                return Binding.DoNothing;

            return categoryNames[index];
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            // ellenőrizzük az értéket
            if (value == null || !(value is String))
                return DependencyProperty.UnsetValue; // ha nem megfelelő, nem csinálunk semmit

            // ellenőrizzük a paramétert
            if (parameter == null || !(parameter is IEnumerable<String>))
                return Binding.DoNothing;

            List<String> categoryNames = (parameter as IEnumerable<String>)!.ToList();
            String name = value.ToString()!;

            // megkeressük a nevet
            if (!categoryNames.Contains(name))
                return DependencyProperty.UnsetValue;

            return name;
        }
    }

    public class BooleanToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool booleanValue = (bool)value;
            return booleanValue ? "Igen" : "Nem";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeToNullableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? dateTimeValue = value as DateTime?;
            if(dateTimeValue == DateTime.MinValue)
            {
                return String.Empty;
            }
            return dateTimeValue!;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MainViewModel : ViewModelBase 
    {
        private readonly FoodOrderAPIService _service;
        private ObservableCollection<OrderViewModel?>? _orders;
        private ObservableCollection<ProductViewModel?>? _products;
        private ObservableCollection<ProductViewModel?>? _allProducts;
        private OrderViewModel? _selectedOrder;
        private ProductViewModel? _selectedProduct;

        public ObservableCollection<OrderViewModel?>? Orders
        {
            get { return _orders; }
            set { _orders = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ProductViewModel?>? Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ProductViewModel?>? AllProducts
        {
            get { return _allProducts; }
            set { _allProducts = value; OnPropertyChanged(); }
        }

        public OrderViewModel? SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
            }
        }

        public ProductViewModel? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        public bool R1 { get; set; }
        public bool R2 { get; set; }
        public bool R3 { get; set; }
        public bool R4 { get; set; }
        public bool R5 { get; set; }
        public string? ProductName { get; set; }
        public int Price { get; set; }
        public string? CategoryNameComboBox { get; set; }
        public string? Description { get; set; }
        public bool SpicyChecked { get; set; }
        public bool VegetarianChecked { get; set; }

        public DelegateCommand SearchCommand { get; private set; }

        public DelegateCommand SelectOrderCommand { get; private set; }

        public DelegateCommand RefreshOrdersCommand { get; private set; }

        public DelegateCommand RefreshProductsCommand { get; private set; }

        public DelegateCommand OrderDoneCommand { get; private set; }

        public DelegateCommand LogoutCommand { get; private set; }

        public DelegateCommand AddProductsCommand { get; private set; }

        public DelegateCommand AddProductCommand { get; private set; }

        public event EventHandler? LogoutSucceeded;

        public event EventHandler? StartProductsAdd;

        public event EventHandler<string>? ProductValidation;

        public MainViewModel(FoodOrderAPIService service)
        {
            _service = service;

            LogoutCommand = new DelegateCommand(_ => LogoutAsync());
            RefreshOrdersCommand = new DelegateCommand(_ => LoadOrdersAsync());
            SelectOrderCommand = new DelegateCommand(_ => LoadProductsAsync(SelectedOrder!));
            OrderDoneCommand = new DelegateCommand(_ => OrderEdit(SelectedOrder!));

            AddProductsCommand = new DelegateCommand(_ => AddPoductWindow());
            RefreshProductsCommand = new DelegateCommand(_ => LoadProductsAsync());

            SearchCommand = new DelegateCommand(param => Search((param as string)!));

            AddProductCommand = new DelegateCommand(_ => AddItem());
        }

        private void AddPoductWindow()
        {
            StartProductsAdd?.Invoke(this, EventArgs.Empty);
        }

        private async void Search(string v)
        {
            if(R1)
            {
                try
                {
                    Orders = new ObservableCollection<OrderViewModel>((await _service.LoadSearchedOrdersAsync(1,v)).Select(list =>
                    {
                        var listVm = (OrderViewModel)list;
                        return listVm;
                    }))!;
                }
                catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
                {
                    OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
                }
            }
            else if(R2)
            {
                try
                {
                    Orders = new ObservableCollection<OrderViewModel>((await _service.LoadSearchedOrdersAsync(2, v)).Select(list =>
                    {
                        var listVm = (OrderViewModel)list;
                        return listVm;
                    }))!;
                }
                catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
                {
                    OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
                }
            }
            else if(R3)
            {
                try
                {
                    Orders = new ObservableCollection<OrderViewModel>((await _service.LoadSearchedOrdersAsync(3, v)).Select(list =>
                    {
                        var listVm = (OrderViewModel)list;
                        return listVm;
                    }))!;
                }
                catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
                {
                    OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
                }
            }
            else if (R4)
            {
                try
                {
                    Orders = new ObservableCollection<OrderViewModel>((await _service.LoadSearchedOrdersAsync(4, v)).Select(list =>
                    {
                        var listVm = (OrderViewModel)list;
                        return listVm;
                    }))!;
                }
                catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
                {
                    OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
                }
            }
            else if(R5)
            {
                R1 = false;
                R2 = false;
                R3 = false;
                R4 = false;
                R5 = false;
                OnPropertyChanged(nameof(R1));
                OnPropertyChanged(nameof(R2));
                OnPropertyChanged(nameof(R3));
                OnPropertyChanged(nameof(R4));
                OnPropertyChanged(nameof(R5));
                LoadOrdersAsync();
            }
        }

        #region Authentication

        private async void LogoutAsync()
        {
            try
            {
                await _service.LogoutAsync();
                OnLogoutSuccess();
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        private void OnLogoutSuccess()
        {
            LogoutSucceeded?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Orders

        private async void LoadOrdersAsync()
        {
            try
            {
                Orders = new ObservableCollection<OrderViewModel>((await _service.LoadOrdersAsync()).Select(list =>
                {
                    var listVm = (OrderViewModel)list;
                    return listVm;
                }))!;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                AllProducts = new ObservableCollection<ProductViewModel>((await _service.LoadAllProductsAsync()).Select(list =>
                {
                    var listVm = (ProductViewModel)list;
                    return listVm;
                }))!;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        private async void OrderEdit(OrderViewModel orderVm)
        {
            if(orderVm == null)
            {
                return;
            }

            try
            {
                if(!orderVm.Done)
                {
                    orderVm.Done = true;
                    orderVm.DoneDate = DateTime.Now;
                    await _service.UpdateOrderAsync((OrderDto)orderVm);
                }
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        #endregion

        #region Products
        private async void LoadProductsAsync(OrderViewModel order)
        {
            if (order is null || order.OrderId == 0)
            {
                Products = null;
                return;
            }

            try
            {
                Products = new ObservableCollection<ProductViewModel>((await _service.LoadProductsAsync(order.OrderId)).Select(item => (ProductViewModel)item))!;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
            }
        }

        private async void AddItem()
        {
            bool notValid = false;
            foreach (var item in AllProducts!)
            {
                if (item!.Name == ProductName)
                {
                    ProductValidation?.Invoke(this, "Már van ilyen nevű étel/ital!");
                    ProductName = "";
                    Price = 0;
                    Description = "";
                    SpicyChecked = false;
                    VegetarianChecked = false;
                    OnPropertyChanged(nameof(ProductName));
                    OnPropertyChanged(nameof(CategoryNameComboBox));
                    OnPropertyChanged(nameof(Price));
                    OnPropertyChanged(nameof(Description));
                    OnPropertyChanged(nameof(SpicyChecked));
                    OnPropertyChanged(nameof(VegetarianChecked));
                    return;
                }
            }
            if(ProductName == "" || ProductName == null)
            {
                if (!notValid)
                {
                    ProductValidation?.Invoke(this, "A hozzáadandó étel/ital neve üres!");
                }
                notValid = true;
            }
            else if (Price <= 0)
            {
                if (!notValid)
                {
                    ProductValidation?.Invoke(this, "A hozzáadandó étel/ital ára nem megfelelő!");
                }
                notValid = true;
            }
            else if(CategoryNameComboBox == null || CategoryNameComboBox == "")
            {
                if (!notValid)
                {
                    ProductValidation?.Invoke(this, "A hozzáadandó étel-nek/ital-nak nem megfelelő a kategóriája!");
                }
                notValid = true;
            }
            else if ((await _service.CategoryIsDrink(CategoryNameComboBox!)) == false && (Description == "" || Description == null))
            {
                if(!notValid)
                {
                    ProductValidation?.Invoke(this, "A hozzáadandó ételnek meg kell adni a leírását!");
                }
                notValid = true;
            }
            else if ((await _service.CategoryIsDrink(CategoryNameComboBox!)) == true && Description != "" && Description != null)
            {
                if (!notValid)
                {
                    ProductValidation?.Invoke(this, "A hozzáadandó üdítőnek tilos megadni leírást!");
                }
                notValid = true;
            }
            else if ((await _service.CategoryIsDrink(CategoryNameComboBox!)) == true && (SpicyChecked == true || VegetarianChecked == true))
            {
                if (!notValid)
                {
                    ProductValidation?.Invoke(this, "A hozzáadandó üdítőnek tilos erősnek vagy vegetáriánusnak lennie!");
                }
                notValid = true;
            }
            
            if(notValid)
            {
                ProductName = "";
                Price = 0;
                Description = "";
                SpicyChecked = false;
                VegetarianChecked = false;
                OnPropertyChanged(nameof(ProductName));
                OnPropertyChanged(nameof(CategoryNameComboBox));
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(SpicyChecked));
                OnPropertyChanged(nameof(VegetarianChecked));
                return;
            }

            var newProduct = new ProductDto
            {
                Name = ProductName!,
                CategoryName = CategoryNameComboBox!,
                Price = Price,
                Description = Description,
                Spicy = SpicyChecked,
                Vegetarian = VegetarianChecked,
            };
            await _service?.CreateProductAsync(newProduct)!;

            await LoadProductsAsync();
            OnPropertyChanged(nameof(AllProducts));

            ProductName = "";
            Price = 0;
            Description = "";
            SpicyChecked = false;
            VegetarianChecked = false;
            OnPropertyChanged(nameof(ProductName));
            OnPropertyChanged(nameof(CategoryNameComboBox));
            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(SpicyChecked));
            OnPropertyChanged(nameof(VegetarianChecked));
        }

        #endregion
    }
}
