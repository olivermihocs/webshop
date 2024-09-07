using DrinkStore.Desktop.Model;
using DrinkStore.Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace DrinkStore.Desktop.ViewModel
{
    public class OrdersViewModel : ViewModelBase
    {

        private readonly DrinkStoreAPIService _service;

        private ObservableCollection<OrderDto> _orders;
        private OrderDto _selectedOrder;
        private ObservableCollection<OrderLineDto> _orderLines;

        private FilterDto _filter;
        private bool _filtered;

        public FilterDto Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<OrderDto> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged();
            }
        }

        public OrderDto SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<OrderLineDto> OrderLines
        {
            get => _orderLines;
            set
            {
                _orderLines = value;
                OnPropertyChanged();
            }
        }

        //Frissítés
        public DelegateCommand RefreshOrdersCommand { get; private set; }

        //Rendelés kiválasztása
        public DelegateCommand SelectOrderCommand { get; private set; }

        //Rendelések szűrése
        public DelegateCommand FilterOrdersCommand { get; private set; }

        //Megerősítés indítása
        public DelegateCommand OnCheckedCommand { get; private set; }

        //Megerősítés
        public DelegateCommand ConfirmationCommand { get; private set; }

        //Megerősítés megszakad
        public DelegateCommand ConfirmationCancelledCommand { get; private set; }

        //Főnézet megnyitása
        public DelegateCommand OpenMainCommand { get; private set; }

        //Kijelentkezés
        public DelegateCommand LogOutCommand { get; private set; }

        //Kilépés
        public DelegateCommand ExitCommand { get; set; }


        public event EventHandler ExitApplication;
        public event EventHandler LogOut;
        public event EventHandler OpenMain;
        public event EventHandler StartOrderConfirmation;
        public event EventHandler EndOrderConfirmation;

        public OrdersViewModel(DrinkStoreAPIService service)
        {

            _service = service;
            _filter = new FilterDto()
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                Name = "",
                Done = true,
                NotDone = true
            };
            _filtered = false;

            RefreshOrdersCommand = new DelegateCommand(_ => LoadOrdersAsync());
            SelectOrderCommand = new DelegateCommand(_ => LoadSelectedOrderAsync());
            FilterOrdersCommand = new DelegateCommand(_ => FilterOrdersAsync());
            OnCheckedCommand = new DelegateCommand(param => OnOrderChecked(param as Int32?));
            ConfirmationCommand = new DelegateCommand(_ => OnOrderConfirmed());
            ConfirmationCancelledCommand = new DelegateCommand(_ => OnEndOrderConfirmation());
            OpenMainCommand = new DelegateCommand(_ => OnOpenMain());
            ExitCommand = new DelegateCommand(param => OnExitApplication());
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

        private void OnOpenMain()
        {
            OpenMain?.Invoke(this, EventArgs.Empty);
        }

        public void OnReload(object sender, EventArgs e)
        {
            _filter = new FilterDto()
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                Name = "",
                Done = true,
                NotDone = true
            };
            _filtered = false;
            LoadOrdersAsync();
        }

        private async void OnOrderConfirmed()
        {
            LoadSelectedOrderAsync();
            try
            {
                if (await _service.UpdateOrderAsync(SelectedOrder.Id, SelectedOrder))
                {
                    OnMessageApplication("Sikeres művelet.");
                }
                else
                {
                    OnMessageApplication("Sikertelen művelet.");
                }
                OnEndOrderConfirmation();
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Hiba történt! ({ex.Message})");
            }
        }

        private void OnEndOrderConfirmation()
        {
            EndOrderConfirmation?.Invoke(this, EventArgs.Empty);
            if (_filtered)
            {
                FilterOrdersAsync();
            }
            else
            {
                LoadOrdersAsync();
            }
        }

        private void OnOrderChecked(Int32? id)
        {
            if(id==null || id != SelectedOrder.Id)
                return;

            OnStartOrderConfirmation();
        }
        
        private void OnStartOrderConfirmation()
        {
            StartOrderConfirmation?.Invoke(this, EventArgs.Empty);
        }


        private async void FilterOrdersAsync()
        {
            _filtered = true;
            try
            {
                Orders = new ObservableCollection<OrderDto>(await _service.LoadFilteredOrdersAsync(_filter));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Hiba történt! ({ex.Message})");
            }
        }

        private async void LoadOrdersAsync()
        {
            OrderLines = null;
            try
            {
                Orders = new ObservableCollection<OrderDto>(await _service.LoadOrdersAsync());
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Hiba történt! ({ex.Message})");
            }
        }
        private async void LoadSelectedOrderAsync()
        {
            try
            {
                if (SelectedOrder != null)
                {

                    IEnumerable<OrderLineDto> orderLines = new ObservableCollection<OrderLineDto>(await _service.LoadOrderLinesByOrderAsync(SelectedOrder.Id));
                    foreach(OrderLineDto orderLine in orderLines)
                    {
                        orderLine.Product = await _service.LoadProductByIdAsync(orderLine.ProductId);
                    }
                    OrderLines = new ObservableCollection<OrderLineDto>(orderLines);
                }

            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Hiba történt! ({ex.Message})");
            }
        }
    }

    
}
