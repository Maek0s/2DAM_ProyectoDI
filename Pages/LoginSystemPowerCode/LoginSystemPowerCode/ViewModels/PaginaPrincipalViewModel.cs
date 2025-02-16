using LoginSystemPowerCode.Models;
using LoginSystemPowerCode.Systems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Timer = System.Timers.Timer;


namespace LoginSystemPowerCode.ViewModels
{
    public class PaginaPrincipalViewModel : INotifyPropertyChanged
    {
        private Timer _timer;
        private int _imagenActual = 0;
        private MgtDatabase _mgtDatabase = new MgtDatabase();

        private List<string> _imagenes = new();
        private List<string> _titulos = new();
        private List<string> _descripciones = new();
        private List<string> _precios = new();

        // Propiedades Bindables
        private string _imagen;
        public string Imagen
        {
            get => _imagen;
            set
            {
                _imagen = value;
                OnPropertyChanged(nameof(Imagen));
            }
        }

        private string _titulo;
        public string Titulo
        {
            get => _titulo;
            set
            {
                _titulo = value;
                OnPropertyChanged(nameof(Titulo));
            }
        }

        private string _descripcionTexto;
        public string DescripcionTexto
        {
            get => _descripcionTexto;
            set
            {
                _descripcionTexto = value;
                OnPropertyChanged(nameof(DescripcionTexto));
            }
        }

        private string _precio;
        public string Precio
        {
            get => _precio;
            set
            {
                _precio = value;
                OnPropertyChanged(nameof(Precio));
            }
        }

        private string _usuarioID;
        public string UsuarioID
        {
            get { return _usuarioID; }
            set
            {
                _usuarioID = value;
            }
        }

        // MENU //

        public ICommand NavegarPerfilCommand { get; }

        public PaginaPrincipalViewModel()
        {
            // Inicializar valores
            ActualizarDatos();

            NavegarPerfilCommand = new Command(viajarPerfil);

            CargarJuegosDesdeBD();

            // Configurar el temporizador para cambiar la imagen cada 10 segundos
            _timer = new Timer(10000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }


        private async void CargarJuegosDesdeBD()
        {
            List<Juego> juegos = _mgtDatabase.ObtenerTodosLosJuegos(); // Suponiendo que esta función existe en MgtDatabase

            if (juegos.Any())
            {
                _imagenes = juegos.Select(j => j.Imagen).ToList();
                _titulos = juegos.Select(j => j.Nombre).ToList();
                _descripciones = juegos.Select(j => j.Descripcion).ToList();
                _precios = juegos.Select(j => $"{j.Precio}€").ToList(); // Formateamos el precio con €

                _imagenActual = 0;
                ActualizarDatos();
            }
        }

        private async void viajarPerfil()
        {
            await Shell.Current.GoToAsync($"/Perfil?usuario={UsuarioID}");
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _imagenActual = (_imagenActual + 1) % _imagenes.Count;

            MainThread.BeginInvokeOnMainThread(ActualizarDatos);
        }

        private void ActualizarDatos()
        {
            if (_imagenes.Count == 0) return;

            Imagen = _imagenes[_imagenActual];
            Titulo = _titulos[_imagenActual];
            DescripcionTexto = _descripciones[_imagenActual];
            Precio = _precios[_imagenActual];
        }

        public void DetenerTemporizador()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
