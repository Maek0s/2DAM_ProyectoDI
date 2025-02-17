using LoginSystemPowerCode.Models;
using LoginSystemPowerCode.Systems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;

namespace LoginSystemPowerCode.ViewModels
{
    public class GestionUsuariosViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Usuario> Usuarios { get; set; }
        private Usuario _usuarioSeleccionado;
        private readonly MgtDatabase _db = new MgtDatabase();

        public Usuario UsuarioSeleccionado
        {
            get => _usuarioSeleccionado;
            set
            {
                _usuarioSeleccionado = value;
                OnPropertyChanged(nameof(UsuarioSeleccionado));
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

        public Command GuardarCambiosCommand { get; }
        public ICommand NavegarPerfilCommand { get; }
        public ICommand ViajarCarteraCommand { get; }
        public ICommand ViajarSalidaCommand { get; }
        public ICommand ViajarHomeCommand { get; }
        public GestionUsuariosViewModel(Page page)
        {
            Usuarios = new ObservableCollection<Usuario>(CargarUsuarios());
            GuardarCambiosCommand = new Command(GuardarCambios);

            // MENU //
            ViajarHomeCommand = new Command(async () => await NavegarID("PaginaPrincipal"));
            ViajarCarteraCommand = new Command(async () => await NavegarID("Cartera"));
            NavegarPerfilCommand = new Command(async () => await NavegarID("Perfil"));
            ViajarSalidaCommand = new Command(async () => await NavegarA("LoginPage"));
        }

        public GestionUsuariosViewModel()
        {

        }


        private async Task NavegarID(string pagina)
        {
            await Shell.Current.GoToAsync($"/{pagina}?usuario={UsuarioID}");
        }
        private async Task NavegarA(string pagina)
        {
            await Shell.Current.GoToAsync($"/{pagina}");
        }

        private List<Usuario> CargarUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (SQLiteConnection connection = new SQLiteConnection(_db.sacarConnection()))
            {
                connection.Open();

                string query = "SELECT idUser, nickname, correo, username, saldo, imagen FROM usuarios";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new Usuario
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("idUser")),
                                Nickname = reader.GetString(reader.GetOrdinal("nickname")),
                                Correo = reader.GetString(reader.GetOrdinal("correo")),
                                Nombre = reader.GetString(reader.GetOrdinal("username")),
                                Saldo = reader.GetInt32(reader.GetOrdinal("saldo")),
                                Imagen = reader.IsDBNull(reader.GetOrdinal("imagen")) ? "avatarlogopowercode.png" : reader.GetString(reader.GetOrdinal("imagen"))
                            });
                        }
                    }
                }

                connection.Close();
            }

            // Verifica si la lista tiene datos
            Debug.WriteLine($"Usuarios cargados: {usuarios.Count}");
            return usuarios;
        }



        private void GuardarCambios()
        {
            if (UsuarioSeleccionado == null) return;

            using (SQLiteConnection connection = new SQLiteConnection(_db.sacarConnection()))
            {
                connection.Open();

                // Actualizamos nickname, username y saldo
                string query = "UPDATE usuarios SET nickname = @nickname, username = @username, saldo = @saldo WHERE idUser = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nickname", UsuarioSeleccionado.Nickname);
                    command.Parameters.AddWithValue("@username", UsuarioSeleccionado.Nombre);
                    command.Parameters.AddWithValue("@saldo", UsuarioSeleccionado.Saldo);
                    command.Parameters.AddWithValue("@id", UsuarioSeleccionado.Id);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            Debug.WriteLine($"Usuario {UsuarioSeleccionado.Nickname} actualizado.");
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        

    }
}
