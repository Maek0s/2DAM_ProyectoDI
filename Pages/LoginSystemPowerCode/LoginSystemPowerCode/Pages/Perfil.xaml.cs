using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using LoginSystemPowerCode.Models;

namespace LoginSystemPowerCode.Pages {
	public partial class Perfil : ContentPage, INotifyPropertyChanged
	{
		private Usuario _usuario;
        public Usuario Usuario
        {
            get { return _usuario; }
            set
            {
                if (_usuario != value)
                {
                    _usuario = value;
                    OnPropertyChanged(nameof(Usuario));
                    OnPropertyChanged(nameof(Usuario.ListaJuegos));
                }
            }
        }

        public ICommand ResetPasswordCommand { get; }

        public Perfil()
		{
			InitializeComponent();

            ResetPasswordCommand = new Command(async () => await ResetPassword());

            testing();

            BindingContext = this;
        }

        private async Task ResetPassword()
        {
            if (string.IsNullOrWhiteSpace(CurrentUser.Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No hay un email registrado.", "OK");
                return;
            }

            var authService = new FirebaseAuthService();
            try
            {
                await authService.SendPasswordResetEmail(CurrentUser.Email);
                await Application.Current.MainPage.DisplayAlert("�xito", "Se ha enviado un correo para restablecer la contrase�a.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }


        public void testing()
		{
            Juego lol = new Juego("League of legends", 20, "Descripci�n", 130, "logolol.png");
            Juego mrivals = new Juego("Marvel Rivals", 40, "Descripci�n", 10, "avatarlogomarvelrivals.jpg");
            ObservableCollection<Juego> listaJuegos = new ObservableCollection<Juego>();
            listaJuegos.Add(lol);
            listaJuegos.Add(mrivals);

            Usuario = new Usuario("Marcos", "Maek0s", "maek0spam@gmail.com", listaJuegos, "logopowercode.png", 100);
            Debug.WriteLine(Usuario.Nombre);
        }

        private async void OnChangeNickname_Tapped(object sender, TappedEventArgs e)
        {
            string nuevoNombre = await DisplayPromptAsync("Editar Nickname", "Introduce tu nuevo nickname:", "OK",
                                                  "Cancelar", "Nuevo nickname", maxLength: 20, keyboard: Keyboard.Text);

            // Si el usuario presiona OK y no deja vac�o el campo
            if (!string.IsNullOrWhiteSpace(nuevoNombre))
            {
                Usuario.Nickname = nuevoNombre;
                OnPropertyChanged(nameof(Usuario)); // Notifica el cambio para actualizar la UI
            }
        }

        private async void OnChangeNombre_Tapped(object sender, TappedEventArgs e)
        {
            string nuevoNombre = await DisplayPromptAsync("Editar Nombre", "Introduce tu nuevo nombre:", "OK",
                                                  "Cancelar", "Nuevo nombre", maxLength: 20, keyboard: Keyboard.Text);

            // Si el usuario presiona OK y no deja vac�o el campo
            if (!string.IsNullOrWhiteSpace(nuevoNombre))
            {
                Usuario.Nombre = nuevoNombre;
                OnPropertyChanged(nameof(Usuario)); // Notifica el cambio para actualizar la UI
            }
        }

        // pendiente de hacer mejor
        private async void OnChangeProfileImage_Tapped(object sender, TappedEventArgs e)
        {
            var popup = new ImageSelectionPopup();
            popup.ImageSelected += (selectedImage) =>
            {
                UpdateProfileImage(selectedImage); // Guarda la imagen seleccionada
            };
            this.ShowPopup(popup); // Muestra el popup
        }

        // M�todo para actualizar la imagen seleccionada
        public void UpdateProfileImage(string imagePath)
        {
            _usuario.Imagen = imagePath;
        }
    }
}