using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemPowerCode.Models
{
    public class Juego : INotifyPropertyChanged
    {
        // Declaración de variables //

        private int id;
        private String _nombre;
        private int _precio;
        private String _descripcion;
        private String _imagen;
        private int _horasJugadas;

        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }

        public String Nombre
        {
            get { return _nombre; }
            set { _nombre = value; OnPropertyChanged(); }
        }

        public int Precio
        {
            get { return _precio; }
            set { _precio = value; OnPropertyChanged(); }
        }

        public String Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; OnPropertyChanged(); }
        }

        public String Imagen
        {
            get { return _imagen; }
            set { _imagen = value; OnPropertyChanged(); }
        }

        public int HorasJugadas
        {
            get { return _horasJugadas; }
            set { _horasJugadas = value; OnPropertyChanged(); }
        }

        // On Property Changed //

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructores //

        /// <summary>
        /// Constructor de la clase Juego
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="precio"></param>
        /// <param name="descripcion"></param>
        /// <param name="horasJugadas"></param>
        /// <param name="imagen"></param>
        public Juego(String nombre, int precio, String descripcion, String imagen)
        {
            Nombre = nombre;
            Precio = precio;
            Descripcion = descripcion;
            Imagen = imagen;
        }

        /// <summary>
        /// Constructor vacío de la clase Juego
        /// </summary>
        public Juego()
        {
        }
    }
}
