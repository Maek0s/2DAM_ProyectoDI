using System.Diagnostics;
using LoginSystemPowerCode.Pages;
using LoginSystemPowerCode.Systems;

// La MainPage esta vacía para una posible pantalla de inicio antes del login
// o si es posible hacer un cache de si esta la cuenta iniciada pasar directamente dentro.

namespace LoginSystemPowerCode
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            
            InitializeComponent();

            inicio();
        }

        public async void inicio()
        {
            MgtDatabase mgtDatabase = new MgtDatabase();
            await Navigation.PushAsync(new LoginPage());
        }
    }

}
