using SQLite;
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
        // Declaración de variables privadas
        private int _id;
        private string _correo;
        private string _nickname;
        private string _nombre;
        private string _password;
        private string _imagen;
        private int _saldo;
        private ObservableCollection<Juego> _listaJuegos;

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string Correo
        {
            get { return _correo; }
            set
            {
                if (_correo != value)
                {
                    _correo = value;
                    OnPropertyChanged(nameof(Correo));
                }
            }
        }

        public string Nickname
        {
            get { return _nickname; }
            set
            {
                if (_nickname != value)
                {
                    _nickname = value;
                    OnPropertyChanged(nameof(Nickname));
                }
            }
        }

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (_nombre != value)
                {
                    _nombre = value;
                    OnPropertyChanged(nameof(Nombre));
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public string Imagen
        {
            get { return _imagen; }
            set
            {
                if (_imagen != value)
                {
                    _imagen = value;
                    OnPropertyChanged(nameof(Imagen));
                }
            }
        }

        public int Saldo
        {
            get { return _saldo; }
            set
            {
                if (_saldo != value)
                {
                    _saldo = value;
                    OnPropertyChanged(nameof(Saldo));
                }
            }
        }

        public ObservableCollection<Juego> ListaJuegos
        {
            get { return _listaJuegos; }
            set
            {
                if (_listaJuegos != value)
                {
                    _listaJuegos = value;
                    OnPropertyChanged(nameof(ListaJuegos));
                }
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
        /// Constructor de la clase Usuario sin imagen
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="nickname"></param>
        /// <param name="correo"></param>
        /// <param name="password"></param>
        /// <param name="saldo"></param>
        /// <param name="listaJuegos"></param>
        public Usuario(String nombre, String nickname, String correo, String password, int saldo, ObservableCollection<Juego> listaJuegos)
        {
            Nombre = nombre;
            Nickname = nickname;
            Correo = correo;
            Password = password;
            Imagen = "avatarlogopowercode.png";
            Saldo = saldo;
        }

        /// <summary>
        /// Constructor de la clase Usuario vacío
        /// </summary>
        public Usuario()
        {
        }
    }
}
