using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using Microsoft.Maui.Dispatching;
using System.Timers;
using Timer = System.Timers.Timer;

namespace PaginaPrincipalProyectoo
{
    public partial class MainPage : ContentPage
    {

        private Timer _timer; // Temporizador para cambiar las imágenes
        private List<string> _imagenes; // Lista de imágenes
        private List<string> _titulos;
        private List<string> _descripcion;
        private List<string> _precios;
        private int _imagenActual = 0; // Índice de la imagen actual

        public MainPage()
        {
            InitializeComponent();

            //La lista de las imagenes que pasan por el carrusel
            _imagenes = new List<string>
        {
            "lol.jpg", // Asegúrate de que las imágenes estén en la carpeta Resources o su ruta sea correcta
            "marvelrivals.jpg",
            "osu.jpg"
        };
            _titulos = new List<String>
            {
                "League of Legends ",
                "Marvel Rivals",
                "Osu"
            };

            _descripcion = new List<String>
            {
                "League of Legens es un videojuego multijugador de arena de batalla ",
                "Marvel Rivals es un videojuego de acción de disparos de héroes en tercera persona  ",
                "Osu es  juego que  está fuertemente orientado a la comunidad, con todos los mapas de ritmos y  canciones jugables."
            };
            _precios = new List<String>
            {
                "Precio: 20€",
                "Precio: 40€ ",
                "Precio: 5€"
            };

                

            // Inicializamos la primera imagen
            imageView.Source = _imagenes[_imagenActual];
            tituloView.Text = _titulos[_imagenActual];
            descripcionView.Text = _descripcion[_imagenActual];
            precioView.Text = _precios[_imagenActual];
            // Configurar el temporizador para cambiar la imagen cada 3 segundos (3000 milisegundos)
            _timer = new Timer(10000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start(); 
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Incrementamos el índice de la imagen actual
            _imagenActual = (_imagenActual + 1) % _imagenes.Count; // Usamos módulo para reiniciar al llegar al final

            // Actualizamos la imagen en el hilo principal
            MainThread.BeginInvokeOnMainThread(() =>
            {
                imageView.Source = _imagenes[_imagenActual];
                tituloView.Text = _titulos[_imagenActual];
                descripcionView.Text = _descripcion[_imagenActual];
                precioView.Text = _precios[_imagenActual];


            });
        }

        // Aseguramos detener el temporizador cuando la página se desactive
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _timer?.Stop();
            _timer?.Dispose();
        }



    }

}
