using FoodOrder.DTO;
using System;


namespace FoodOrder.Desktop.ViewModel
{
    public class ProductViewModel : ViewModelBase
    {
        private Int32 _id;
        private String? _name;
        private String? _categoryName;
        private String? _description;
        private int _price;
        private bool _spicy;
        private bool _vegetarian;


        public String this[string columnName]
        {
            get
            {
                String error = String.Empty;
                switch (columnName)
                {
                    case nameof(Name):
                        if (String.IsNullOrEmpty(Name))
                            error = "Item name cannot be empty.";
                        else if (Name.Length > 30)
                            error = "Item name cannot be longer than 30 characters.";
                        break;
                }
                return error;
            }
        }

        public Int32 Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public String Name
        {
            get => _name!;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public String CategoryName
        {
            get => _categoryName!;
            set
            {
                _categoryName = value;
                OnPropertyChanged();
            }
        }

        public String Description
        {
            get => _description!;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public int Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        public bool Spicy
        {
            get => _spicy;
            set
            {
                _spicy = value;
                OnPropertyChanged();
            }
        }

        public bool Vegetarian
        {
            get => _vegetarian;
            set
            {
                _vegetarian = value;
                OnPropertyChanged();
            }
        }

        public static explicit operator ProductViewModel(ProductDto dto) => new ProductViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            CategoryName = dto.CategoryName,
            Description = dto.Description!,
            Price = dto.Price,
            Spicy = dto.Spicy,
            Vegetarian= dto.Vegetarian,
        };

        public static explicit operator ProductDto(ProductViewModel vm) => new ProductDto
        {
            Id = vm.Id,
            Name = vm.Name,
            CategoryName = vm.CategoryName,
            Description = vm.Description,
            Price = vm.Price,
            Spicy = vm.Spicy,
            Vegetarian = vm.Vegetarian,
        };
    }
}
