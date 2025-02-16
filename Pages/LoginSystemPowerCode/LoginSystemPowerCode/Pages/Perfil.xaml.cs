using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using LoginSystemPowerCode.Models;
using LoginSystemPowerCode.Systems;
using LoginSystemPowerCode.ViewModels;

namespace LoginSystemPowerCode.Pages {
    [QueryProperty("UsuarioID", "usuario")]
    public partial class Perfil : ContentPage, INotifyPropertyChanged
	{
        private readonly MgtDatabase mgtDatabase = new MgtDatabase();
        private readonly PerfilViewModel _viewModel;
        public string UsuarioID { get; set; }  // Propiedad para recibir el parámetro

        public Perfil()
        {
            InitializeComponent();
            _viewModel = new PerfilViewModel(this);
            BindingContext = _viewModel;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            if (!string.IsNullOrWhiteSpace(UsuarioID))
            {
                _viewModel.UsuarioID = UsuarioID;
            }
        }

        // Si no esta en este archivo no funciona correctamente.
        private async void OnChangeProfileImage_Tapped(object sender, TappedEventArgs e)
        {
            var popup = new ImageSelectionPopup();
            popup.ImageSelected += (selectedImage) =>
            {
                _viewModel.UpdateProfileImage(selectedImage); // Guarda la imagen seleccionada
            };
            this.ShowPopup(popup); // Muestra el popup
        }

        private async void OnChangeNickname_Tapped(object sender, TappedEventArgs e)
        {
            if (_viewModel.ChangeNicknameCommand.CanExecute(null))
            {
                _viewModel.ChangeNicknameCommand.Execute(null);
            }
        }

        private void OnChangeNombre_Tapped(object sender, EventArgs e)
        {
            if (BindingContext is PerfilViewModel viewModel)
            {
                viewModel.ChangeNombreCommand.Execute(null);
            }
        }
    }
}