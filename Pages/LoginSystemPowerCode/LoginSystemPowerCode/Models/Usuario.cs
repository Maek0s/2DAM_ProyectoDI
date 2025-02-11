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

        private String _nickname;
        public String Nickname
        {
            get { return _nickname; }
            set { _nickname = value; OnPropertyChanged(); }
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

        public int CantidadJuegos => ListaJuegos?.Count ?? 0;

        public int HorasJugadasTotales => ListaJuegos?.Sum(j => j.HorasJugadas) ?? 0;

        private ObservableCollection<Juego> _listaJuegos;
        public ObservableCollection<Juego> ListaJuegos {
            get { return _listaJuegos; } 
            set 
            {
                _listaJuegos = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CantidadJuegos));
                OnPropertyChanged(nameof(HorasJugadasTotales));
            } 
        }

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
        /// <param name="nickname">Apellido del usuario</param>
        /// <param name="correo">Correo del usuario</param>
        /// <param name="imagen">Nombre de la imagen del usuario</param>

        public Usuario(String nombre, String nickname, String correo, String imagen)
        {
            Nombre = nombre;
            Nickname = nickname;
            Correo = correo;
            Imagen = imagen;
        }

        /// <summary>
        /// Constructor de la clase Usuario
        /// </summary>
        /// <param name="nombre">Nombre del usuario</param>
        /// <param name="nickname">Nickname del usuario</param>
        /// <param name="correo">Correo del usuario</param>
        /// <param name="imagen">Nombre de la imagen del usuario</param>

        public Usuario(String nombre, String nickname, String correo)
        {
            Nombre = nombre;
            Nickname = nickname;
            Correo = correo;
            /* Elegir random entre todas
             * 
             * Imagen = ;
            */
        }

        public Usuario(String nombre, String nickname, String correo, ObservableCollection<Juego> listaJuegos, String imagen)
        {
            Nombre = nombre;
            Nickname = nickname;
            Correo = correo;
            ListaJuegos = listaJuegos;
            Imagen = imagen;
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
