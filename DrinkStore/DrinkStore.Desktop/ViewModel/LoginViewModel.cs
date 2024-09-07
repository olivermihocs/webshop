using DrinkStore.Desktop.Model;
using System;
using System.Windows.Controls;

namespace DrinkStore.Desktop.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly DrinkStoreAPIService _service;

        private bool _isLoading;
        public Boolean IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Kilépés parancsának lekérdezése.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// Bejelentkezés parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoginCommand { get; private set; }

        /// <summary>
        /// Felhasználónév lekérdezése, vagy beállítása.
        /// </summary>
        private String _userName { get; set; }

        public String UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Alkalmazásból való kilépés eseménye.
        /// </summary>
        public event EventHandler ExitApplication;

        /// <summary>
        /// Sikeres bejelentkezés eseménye.
        /// </summary>
        public event EventHandler LoginSucceeded;

        /// <summary>
        /// Sikertelen bejelentkezés eseménye.
        /// </summary>
        public event EventHandler LoginFailed;

        /// <summary>
        /// Nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell.</param>
        public LoginViewModel(DrinkStoreAPIService service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            _service = service;
            UserName = String.Empty;
            IsLoading = false;

            ExitCommand = new DelegateCommand(param => OnExitApplication());

            LoginCommand = new DelegateCommand(_ => !IsLoading, param => LoginAsync(param as PasswordBox));
        }

        /// <summary>
        /// Bejelentkezés
        /// </summary>
        /// <param name="passwordBox">Jelszótároló vezérlő.</param>
        private async void LoginAsync(PasswordBox passwordBox)
        {
            
            if (passwordBox == null)
                return;
            try
            {
                // a bejelentkezéshez szükségünk van a jelszótároló vezérlőre, mivel a jelszó tulajdonság nem köthető
                IsLoading = true;
                Boolean result = await _service.LoginAsync(UserName, passwordBox.Password);
                IsLoading = false;

                if (result)
                {
                    UserName = String.Empty;
                    passwordBox.Password = String.Empty;
                    OnLoginSucceeded();
                }
                else
                    OnLoginFailed();
            }
            catch(NetworkException ex)
            {
                OnMessageApplication("Nincs kapcsolat a kiszolgálóval. "+ex.Message);
            }
        }


        /// <summary>
        /// Sikeres bejelentkezés eseménykiváltása.
        /// </summary>
        private void OnLoginSucceeded()
        {
            LoginSucceeded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Alkalmazásból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitApplication()
        {
            ExitApplication?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sikertelen bejelentkezés eseménykiváltása.
        /// </summary>
        private void OnLoginFailed()
        {
            LoginFailed?.Invoke(this, EventArgs.Empty);
        }
    }
}
