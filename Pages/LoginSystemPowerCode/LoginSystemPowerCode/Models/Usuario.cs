using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemPowerCode.Models
{
    public class Usuario : INotifyPropertyChanged
    {
        // Declaración de variables //
        private String _nombre;
        public String Nombre
        {
            get { return _nombre; }
            set { _nombre = value; OnPropertyChanged(); }
        }

        private String _apellido;
        public String Apellido
        {
            get { return _apellido; }
            set { _apellido = value; OnPropertyChanged(); }
        }

        private String _correo;
        public String Correo
        {
            get { return _correo; }
            set { _correo = value; OnPropertyChanged(); }
        }

        private String _imagen;
        public String Imagen
        {
            get { return _imagen; }
            set { _imagen = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Juego> _listaJuegos;
        public ObservableCollection<Juego> listaJuegos { get; set; }

        // On Property Changed //
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructores //

        /// <summary>
        /// Constructor de la clase Usuario
        /// </summary>
        /// <param name="nombre">Nombre del usuario</param>
        /// <param name="apellido">Apellido del usuario</param>
        /// <param name="correo">Correo del usuario</param>
        /// <param name="imagen">Nombre de la imagen del usuario</param>

        public Usuario(String nombre, String apellido, String correo, String imagen)
        {
            Nombre = nombre;
            Apellido = apellido;
            Correo = correo;
            Imagen = imagen;
        }

        /// <summary>
        /// Constructor de la clase Usuario
        /// </summary>
        /// <param name="nombre">Nombre del usuario</param>
        /// <param name="apellido">Apellido del usuario</param>
        /// <param name="correo">Correo del usuario</param>
        /// <param name="imagen">Nombre de la imagen del usuario</param>

        public Usuario(String nombre, String apellido, String correo)
        {
            Nombre = nombre;
            Apellido = apellido;
            Correo = correo;
            /* Elegir random entre todas
             * 
             * Imagen = ;
            */
        }

        /// <summary>
        /// Constructor vacío de la clase Usuario
        /// </summary>
        public Usuario()
        {
        }
    
        public String getRandomImage()
        {
            Random random = new Random();
            int randomNumber = random.Next(1, 5);
            return "image" + randomNumber + ".png";
        }
    }
}
