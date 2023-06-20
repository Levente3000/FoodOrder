using FoodOrder.DTO;
using System;

namespace FoodOrder.Desktop.ViewModel
{
    public class OrderViewModel : ViewModelBase
    {
        private int _orderId;
        private string? _ordererName;
        private string? _address;
        private string? _phoneNumber;
        private bool _done;
        private DateTime _registrationDate;
        private DateTime _doneDate;
        private int _sumPrice;

        public int OrderId
        {
            get => _orderId;
            set
            {
                _orderId = value;
                OnPropertyChanged();
            }
        }
        public string OrdererName
        {
            get => _ordererName!;
            set
            {
                _ordererName = value;
                OnPropertyChanged();
            }
        }
        public string Address
        {
            get => _address!;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }
        public string PhoneNumber
        {
            get => _phoneNumber!;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }
        public bool Done
        {
            get => _done;
            set
            {
                _done = value;
                OnPropertyChanged();
            }
        }
        public DateTime RegistrationDate
        {
            get => _registrationDate;
            set
            {
                _registrationDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime DoneDate
        {
            get => _doneDate;
            set
            {
                _doneDate = value;
                OnPropertyChanged();
            }
        }

        public int SumPrice
        {
            get => _sumPrice;
            set
            {
                _sumPrice = value;
                OnPropertyChanged();
            }
        }

        public static explicit operator OrderViewModel(OrderDto dto) => new OrderViewModel
        {
            OrderId = dto.OrderId,
            OrdererName = dto.OrdererName,
            Address = dto.Address,
            PhoneNumber = dto.PhoneNumber,
            Done = dto.Done,
            RegistrationDate = dto.RegistrationDate,
            DoneDate = dto.DoneDate,
            SumPrice = dto.SumPrice,
        };

        public static explicit operator OrderDto(OrderViewModel vm) => new OrderDto
        {
            OrderId = vm.OrderId,
            OrdererName = vm.OrdererName,
            Address = vm.Address,
            PhoneNumber = vm.PhoneNumber,
            Done = vm.Done,
            RegistrationDate = vm.RegistrationDate,
            DoneDate = vm.DoneDate,
            SumPrice = vm.SumPrice,
        };
    }
}
