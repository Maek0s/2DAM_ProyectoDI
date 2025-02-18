using System.Collections.ObjectModel;
using System.Diagnostics;
using LoginSystemPowerCode.Models;
using LoginSystemPowerCode.Pages;
using LoginSystemPowerCode.Systems;
using LoginSystemPowerCode.ViewModels;

namespace LoginSystemPowerCode.Pages {
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            var viewModel = new LoginPageViewModel();
            BindingContext = viewModel;
        }

        private async void OnLabelClicked(object sender, EventArgs e)
        {
            // Navegar a la página de registro
            await Navigation.PushAsync(new RegisterPage());
        }
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            // Navegar a la página de registro
           
        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            await Task.Delay(1000);  // Espera un poco antes de hacer el foco
            eUsername.Focus();
        }
    }
}