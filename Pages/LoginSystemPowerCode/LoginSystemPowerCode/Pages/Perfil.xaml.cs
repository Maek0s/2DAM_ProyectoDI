using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
                }
            }
        }

        public Perfil()
		{
			InitializeComponent();

			BindingContext = this;

            testing();
		}

		public void testing()
		{
            Juego lol = new Juego("League of legends", 20, "Descripción", 130, "logolol.png");
            Juego mrivals = new Juego("Marvel Rivals", 40, "Descripción", 10, "avatarlogomarvelrivals.jpg");
            ObservableCollection<Juego> listaJuegos = new ObservableCollection<Juego>();
            listaJuegos.Add(lol);
            listaJuegos.Add(mrivals);

            Usuario = new Usuario("Marcos", "Maek0s", "maek0spam@gmail.com", listaJuegos, "logopowercode.png");
            Debug.WriteLine(Usuario.Nombre);
        }

        private async void OnChangeNickname_Tapped(object sender, TappedEventArgs e)
        {
            string nuevoNombre = await DisplayPromptAsync("Editar Nickname", "Introduce tu nuevo nickname:", "OK",
                                                  "Cancelar", "Nuevo nickname", maxLength: 20, keyboard: Keyboard.Text);

            // Si el usuario presiona OK y no deja vacío el campo
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

            // Si el usuario presiona OK y no deja vacío el campo
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

        // Método para actualizar la imagen seleccionada
        public void UpdateProfileImage(string imagePath)
        {
            _usuario.Imagen = imagePath;
        }
    }
}