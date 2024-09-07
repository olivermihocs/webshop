using DrinkStore.Desktop.Model;
using DrinkStore.Desktop.ViewModel;
using DrinkStore.Desktop.View;
using System;
using System.Configuration;
using System.Windows;

namespace DrinkStore.Desktop
{
    public partial class App : Application
    {
        private DrinkStoreAPIService _service;

        //Nézetmodellek
        private LoginViewModel _loginViewModel;
        private MainViewModel _mainViewModel;
        private OrdersViewModel _ordersViewModel;
        private NewProductViewModel _newProductViewModel;

        //Nézetek (ablakok)
        private LoginWindow _loginView;
        private MainWindow _mainView;
        private OrdersWindow _ordersView;
        private NewProductWindow _newProductView;
        private OrderConfirmationWindow _orderConfirmationView;

        public event EventHandler Main_Reload;
        public event EventHandler Orders_Reload;

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _service = new DrinkStoreAPIService(ConfigurationManager.AppSettings["baseAddress"]);

            //Fő ablak
            _mainViewModel = new MainViewModel(_service);
            _mainViewModel.OpenOrders += new EventHandler(MainViewModel_OpenOrders);
            _mainViewModel.CreateNewProductStarted += new EventHandler<CategoryEventArgs>(MainViewModel_OnCreateNewProductStarted);
            _mainViewModel.MessageApplication += new EventHandler<MessageEventArgs>(ViewModel_MessageApplication);
            _mainViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);
            _mainViewModel.LogOut += new EventHandler(ViewModel_LogOut);

            _mainView = new MainWindow
            {
                DataContext = _mainViewModel
            };

            //Rendelések ablak
            _ordersViewModel = new OrdersViewModel(_service);
            _ordersViewModel.OpenMain += new EventHandler(OrdersViewModel_OpenMain);
            _ordersViewModel.MessageApplication += new EventHandler<MessageEventArgs>(ViewModel_MessageApplication);
            _ordersViewModel.StartOrderConfirmation += new EventHandler(OrdersViewModel_OnStartOrderConfirmation);
            _ordersViewModel.EndOrderConfirmation += new EventHandler(OrdersViewModel_OnEndOrderConfirmation);
            _ordersViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);
            _ordersViewModel.LogOut += new EventHandler(ViewModel_LogOut);
            _ordersView = new OrdersWindow
            {
                DataContext = _ordersViewModel
            };

            Main_Reload += new EventHandler(_mainViewModel.OnReload);
            Orders_Reload += new EventHandler(_ordersViewModel.OnReload);

            //Bejelentkező ablak
            _loginViewModel = new LoginViewModel(_service);
            _loginViewModel.LoginSucceeded += ViewModel_LoginSucceeded;
            _loginViewModel.LoginFailed += ViewModel_LoginFailed;
            _loginViewModel.ExitApplication += ViewModel_ExitApplication;
            _loginViewModel.MessageApplication += ViewModel_MessageApplication;
            _loginView = new LoginWindow
            {
                DataContext = _loginViewModel
            };
            _loginView.Show();
        }

        //Kilépés az alkalmazásból
        public async void App_Exit(object sender, ExitEventArgs e)
        {
            if (_service.IsUserLoggedIn)
            {
                await _service.LogoutAsync();
            }
        }

        private void On_Main_Reload()
        {
            Main_Reload?.Invoke(this, EventArgs.Empty);
        }

        private void On_Orders_Reload()
        {
            Orders_Reload?.Invoke(this, EventArgs.Empty);
        }

        //Kilépés
        private void ViewModel_ExitApplication(object sender, EventArgs e)
        {
            Shutdown();
        }

        //Kijelentkezés
        private void ViewModel_LogOut(object sender, EventArgs e)
        {
            _mainView.Hide();
            _ordersView.Hide();
            _loginView.Show();
        }

        //Sikeres Bejelentkezés
        private void ViewModel_LoginSucceeded(object sender, EventArgs e)
        {
            _loginView.Hide();
            On_Main_Reload();
            _mainView.Show();
        }

        //Sikertelen Bejelentkezés
        private void ViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("A bejelentkezés nem sikerült!", "DrinkStore", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        //Az ablakok által küldött üzenetek kezelése egy üzenetdoboz megjelenítésével történik
        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "DrinkStore", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }


        //Új termék felvételéhez szolgáló ablak megjelenítése
        private void MainViewModel_OnCreateNewProductStarted(object sender, CategoryEventArgs e)
        {
            _newProductViewModel = new NewProductViewModel(_service,e.CategoryId);

            _newProductViewModel.EndNewProduct += new EventHandler(NewProductViewModel_EndNewProduct);
            _newProductViewModel.EndNewProduct += new EventHandler(_mainViewModel.OnCreateNewProductEnded);
            _newProductViewModel.MessageApplication += new EventHandler<MessageEventArgs>(ViewModel_MessageApplication);
            

            _newProductView = new NewProductWindow()
            {
                DataContext = _newProductViewModel,
                Title = "DrinkStore - "+ e.CategoryName +" - új termék hozzáadása"
            };

            _newProductView.ShowDialog();
        }

        //Új termék felvételéhez szolgáló ablak bezárása
        private void NewProductViewModel_EndNewProduct(object sender, EventArgs e)
        {
            _newProductView.Close();
        }

        //Egy rendelés megerősítéséhez szolgáló ablak megjelenítése
        private void OrdersViewModel_OnStartOrderConfirmation(object sender, EventArgs e)
        {
            if (_ordersViewModel == null)
                return;

            _orderConfirmationView = new OrderConfirmationWindow()
            {
                DataContext = _ordersViewModel
            };
            _orderConfirmationView.ShowDialog();

        }

        //Egy rendelés megerősítéséhez szolgáló ablak bezárása
        private void OrdersViewModel_OnEndOrderConfirmation(object sender, EventArgs e)
        {
            if (_orderConfirmationView != null)
            {
                _orderConfirmationView.Close();
            }
        }

        //Váltás a rendelés nézetre
        private void MainViewModel_OpenOrders(object sender, EventArgs e)
        {
            _mainView.Hide();
            On_Orders_Reload();
            _ordersView.Show();
        }

        //Váltás a termék nézetre
        private void OrdersViewModel_OpenMain(object sender, EventArgs e)
        {
            _ordersView.Hide();
            On_Main_Reload();
            _mainView.Show();
        }
    }
}
