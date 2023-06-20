using Accessibility;
using FoodOrder.Desktop.Model;
using FoodOrder.Desktop.View;
using FoodOrder.Desktop.ViewModel;
using System;
using System.Configuration;
using System.Windows;

namespace FoodOrder.Desktop
{
    public partial class App : Application
    {
        private FoodOrderAPIService? _service;
        private MainViewModel? _mainViewModel;
        private LoginViewModel? _loginViewModel;
        private MainWindow? _mainView;
        private LoginWindow? _loginView;
        private AddProductWindow? _addProductView;

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _service = new FoodOrderAPIService(ConfigurationManager.AppSettings["baseAddress"]!);

            _loginViewModel = new LoginViewModel(_service);

            _loginViewModel.LoginSucceeded += ViewModel_LoginSucceeded;
            _loginViewModel.LoginFailed += ViewModel_LoginFailed;
            _loginViewModel.MessageApplication += ViewModel_MessageApplication;

            _loginView = new LoginWindow
            {
                DataContext = _loginViewModel
            };

            _mainViewModel = new MainViewModel(_service);
            _mainViewModel.LogoutSucceeded += ViewModel_LogoutSucceeded;
            _mainViewModel.MessageApplication += ViewModel_MessageApplication;
            _mainViewModel.StartProductsAdd += MainViewModelOnAddProducts;
            _mainViewModel.ProductValidation += OnProductValidationError;

            _mainView = new MainWindow
            {
                DataContext = _mainViewModel
            };

            MainWindow = _loginView;
            ShutdownMode = ShutdownMode.OnMainWindowClose;

            _loginView.Closed += LoginView_Closed;

            _loginView.Show();
        }

        private void OnProductValidationError(object? sender, string e)
        {
            MessageBox.Show(e, "FoodOrder", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MainViewModelOnAddProducts(object? sender, EventArgs e)
        {
            _addProductView = new AddProductWindow()
            {
                DataContext = _mainViewModel
            };
            _addProductView.ShowDialog();
        }

        private void LoginView_Closed(object? sender, EventArgs e)
        {
            Shutdown();
        }

        private void ViewModel_LoginSucceeded(object? sender, EventArgs e)
        {
            _loginView?.Hide();
            _mainView?.Show();
        }

        private void ViewModel_LoginFailed(object? sender, EventArgs e)
        {
            MessageBox.Show("Bejelentkezés sikertelen!", "FoodOrder", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_LogoutSucceeded(object? sender, EventArgs e)
        {
            _mainView?.Hide();
            _loginView?.Show();
        }

        private void ViewModel_MessageApplication(object? sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "FoodOrder", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

    }
}
