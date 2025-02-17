using LoginSystemPowerCode.Models;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Diagnostics;

namespace LoginSystemPowerCode.Systems
{
    public class MgtDatabase
    {
        public MgtDatabase()
        {
            CreacionTablas();
        }

        public void testing()
        {
            //InsertarUsuario("maek0spam@gmail.com", "maek0spam", "maek0spam", "1234", "avatarlogopowercode.png");
        }

        private void CreacionTablas()
        {
            CrearTablaUsuario();
            CrearTablaJuegos();
            CrearTablaUsuariosJuegos();
            CrearTablaRoles();
            CrearTablaUsuariosRoles();
        }

        public string IniciarSesion(string correo, string password)
        {
            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();

                // Creamos la consulta para verificar si el correo y la contraseña coinciden
                string query = "SELECT COUNT(*) FROM usuarios WHERE correo = @correo AND password = @password";
                
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    // Agregamos los parámetros para evitar inyecciones SQL
                    command.Parameters.AddWithValue("@correo", correo);
                    command.Parameters.AddWithValue("@password", password);

                    long count = (long)command.ExecuteScalar();

                    if (count > 0)
                    {
                        // Si se encuentra una coincidencia, el inicio de sesión es exitoso
                        connection.Close();
                        return "";
                    }
                    else
                    {
                        // Si no se encuentra el correo o la contraseña, el inicio de sesión falla
                        connection.Close();
                        return "Correo o contraseña incorrectos.";
                    }
                }
            }
        }

        public string CrearUsuario(string correo, string password)
        {
            Debug.WriteLine($"Creando usuario: {correo}");
            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();

                // Verificar si el correo ya existe
                string checkQuery = "SELECT COUNT(*) FROM usuarios WHERE correo = @correo";
                using (SQLiteCommand checkCommand = new SQLiteCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@correo", correo);
                    long count = (long)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        return $"El correo {correo} ya está registrado.";
                    }
                }

                // Consulta de inserción en usuarios
                string query = "INSERT INTO usuarios (correo, nickname, username, password, imagen, saldo) " +
                               "VALUES (@correo, @nickname, @username, @password, @imagen, 10);";

                var nickname = ObtenerUsuarioDeCorreo(correo);
                var username = ObtenerUsuarioDeCorreo(correo);
                var imagen = "avatarlogopowercode.png";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@correo", correo);
                    command.Parameters.AddWithValue("@nickname", nickname);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@imagen", imagen);
                    command.ExecuteNonQuery();
                }

                // Obtener el id del nuevo usuario
                long newUserId = connection.LastInsertRowId;

                // Insertar en la tabla de relación con rol por defecto (id_rol = 1)
                string insertRoleQuery = "INSERT INTO usuarios_roles (id_usuario, id_rol) VALUES (@id_usuario, @id_rol);";
                using (SQLiteCommand roleCommand = new SQLiteCommand(insertRoleQuery, connection))
                {
                    roleCommand.Parameters.AddWithValue("@id_usuario", newUserId);
                    roleCommand.Parameters.AddWithValue("@id_rol", 1);
                    roleCommand.ExecuteNonQuery();
                }

                connection.Close();
                
            }

            return "Cuenta creada exitosamente.";
        }

        public Usuario ObtenerUsuarioPorId(int idUser)
        {
            Usuario usuario = null;
            ObservableCollection<Juego> listaJuegos = new ObservableCollection<Juego>();

            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();

                // Consulta para obtener la información del usuario y sus juegos
                string query = @"
            SELECT u.idUser, u.correo, u.nickname, u.username, u.password, u.imagen, u.saldo, 
                   j.id AS juegoId, j.titulo, uj.horas, uj.imagen AS juego_imagen
            FROM usuarios u
            LEFT JOIN usuarios_juegos uj ON u.idUser = uj.usuario_id
            LEFT JOIN juegos j ON uj.juego_id = j.id
            WHERE u.idUser = @idUser";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUser", idUser);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("idUser")),
                                Correo = reader.GetString(reader.GetOrdinal("correo")),
                                Nickname = reader.GetString(reader.GetOrdinal("nickname")),
                                Nombre = reader.GetString(reader.GetOrdinal("username")),
                                Password = reader.GetString(reader.GetOrdinal("password")),
                                Imagen = reader.GetString(reader.GetOrdinal("imagen")),
                                Saldo = reader.GetInt32(reader.GetOrdinal("saldo")),
                                Admin = false // Valor por defecto
                            };

                            do
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("juegoId")))
                                {
                                    listaJuegos.Add(new Juego
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("juegoId")),
                                        Nombre = reader.GetString(reader.GetOrdinal("titulo")),
                                        Precio = reader.IsDBNull(reader.GetOrdinal("precio")) ? 0 : reader.GetInt32(reader.GetOrdinal("precio")),
                                        Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? "" : reader.GetString(reader.GetOrdinal("descripcion")),
                                        Imagen = reader.IsDBNull(reader.GetOrdinal("juego_imagen")) ? "" : reader.GetString(reader.GetOrdinal("juego_imagen")),
                                        HorasJugadas = reader.IsDBNull(reader.GetOrdinal("horas")) ? 0 : reader.GetInt32(reader.GetOrdinal("horas"))
                                    });
                                }
                            } while (reader.Read());

                            usuario.ListaJuegos = listaJuegos;
                        }
                    }
                }

                // Verificar si el usuario es admin consultando la tabla usuarios_roles
                if (usuario != null)
                {
                    string rolesQuery = "SELECT id_rol FROM usuarios_roles WHERE id_usuario = @usuario_id";
                    using (SQLiteCommand rolesCommand = new SQLiteCommand(rolesQuery, connection))
                    {
                        rolesCommand.Parameters.AddWithValue("@usuario_id", idUser);
                        using (SQLiteDataReader rolesReader = rolesCommand.ExecuteReader())
                        {
                            while (rolesReader.Read())
                            {
                                int role = rolesReader.GetInt32(rolesReader.GetOrdinal("id_rol"));
                                if (role == 2)
                                {
                                    usuario.Admin = true;
                                    break;
                                } else
                                {
                                    usuario.Admin = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                connection.Close();
            }

            return usuario;
        }


        public Usuario ObtenerUsuarioPorCorreo(string correo)
        {
            Usuario usuario = null;
            ObservableCollection<Juego> listaJuegos = new ObservableCollection<Juego>();

            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();

                // Consulta para obtener la información del usuario y sus juegos
                string query = @"
            SELECT u.idUser, u.correo, u.nickname, u.username, u.password, u.imagen, u.saldo, 
                   j.id AS juegoId, j.titulo, uj.horas, uj.imagen AS juego_imagen
            FROM usuarios u
            LEFT JOIN usuarios_juegos uj ON u.idUser = uj.usuario_id
            LEFT JOIN juegos j ON uj.juego_id = j.id
            WHERE u.correo = @correo";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@correo", correo);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuario
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("idUser")),
                                Correo = reader.GetString(reader.GetOrdinal("correo")),
                                Nickname = reader.GetString(reader.GetOrdinal("nickname")),
                                Nombre = reader.GetString(reader.GetOrdinal("username")),
                                Password = reader.GetString(reader.GetOrdinal("password")),
                                Imagen = reader.GetString(reader.GetOrdinal("imagen")),
                                Saldo = reader.GetInt32(reader.GetOrdinal("saldo")),
                                Admin = false // Valor por defecto
                            };

                            do
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("juegoId")))
                                {
                                    listaJuegos.Add(new Juego
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("juegoId")),
                                        Nombre = reader.GetString(reader.GetOrdinal("titulo")),
                                        Precio = reader.IsDBNull(reader.GetOrdinal("precio")) ? 0 : reader.GetInt32(reader.GetOrdinal("precio")),
                                        Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? "" : reader.GetString(reader.GetOrdinal("descripcion")),
                                        Imagen = reader.IsDBNull(reader.GetOrdinal("juego_imagen")) ? "" : reader.GetString(reader.GetOrdinal("juego_imagen")),
                                        HorasJugadas = reader.IsDBNull(reader.GetOrdinal("horas")) ? 0 : reader.GetInt32(reader.GetOrdinal("horas"))
                                    });
                                }
                            } while (reader.Read());

                            usuario.ListaJuegos = listaJuegos;
                        }
                    }
                }

                // Verificar si el usuario es admin consultando la tabla usuarios_roles
                if (usuario != null)
                {
                    string rolesQuery = "SELECT id_rol FROM usuarios_roles WHERE id_usuario = @usuario_id";
                    using (SQLiteCommand rolesCommand = new SQLiteCommand(rolesQuery, connection))
                    {
                        rolesCommand.Parameters.AddWithValue("@usuario_id", usuario.Id);
                        using (SQLiteDataReader rolesReader = rolesCommand.ExecuteReader())
                        {
                            while (rolesReader.Read())
                            {
                                int role = rolesReader.GetInt32(rolesReader.GetOrdinal("id_rol"));
                                if (role == 2)
                                {
                                    usuario.Admin = true;
                                    break;
                                } else
                                {
                                    usuario.Admin = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                connection.Close();
            }

            return usuario;
        }


        /* Métodos para actualizar datos */

        // Método para actualizar la imagen de usuario
        public void ActualizarImagenUsuario(int usuarioId, string nuevaImagen)
        {
            var query = "UPDATE usuarios SET imagen = '" + nuevaImagen + "' WHERE idUser = " + usuarioId;

            ejecutarQuery(query);
        }

        // Método para actualizar el nombre de usuario
        public void ActualizarNicknameUsuario(int usuarioId, string nuevoNickname)
        {
            var query = "UPDATE usuarios SET nickname = '" + nuevoNickname + "' WHERE idUser = " + usuarioId;

            ejecutarQuery(query);
        }

        // Método para actualizar el nombre de usuario
        public void ActualizarUsernameUsuario(int usuarioId, string nuevoUsername)
        {
            var query = "UPDATE usuarios SET username = '" + nuevoUsername + "' WHERE idUser = " + usuarioId;

            ejecutarQuery(query);
        }

        // Método para actualizar la contraseña de usuario
        public void ActualizarPasswordUsuario(int usuarioId, string nuevaPassword)
        {
            var query = "UPDATE usuarios SET password = '" + nuevaPassword + "' WHERE idUser = " + usuarioId;

            ejecutarQuery(query);
        }

        public List<Juego> ObtenerTodosLosJuegos()
        {
            List<Juego> juegos = new List<Juego>();
            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();
                string query = "SELECT id, titulo, descripcion, precio, imagen FROM juegos";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            juegos.Add(new Juego
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Descripcion = reader.GetString(2),
                                Precio = reader.GetInt32(3),
                                Imagen = reader.GetString(4)
                            });
                        }
                    }
                }
                connection.Close();
            }
            return juegos;
        }

        public void ActualizarSaldoUsuario(int userId, int nuevoSaldo)
        {
            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();

                string query = "UPDATE usuarios SET saldo = @saldo WHERE idUser = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@saldo", nuevoSaldo);
                    command.Parameters.AddWithValue("@id", userId);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public string ObtenerImagenPorId(int juegoId)
        {
            string imagen = string.Empty;
            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();

                // Consulta para obtener la imagen correspondiente
                string query = "SELECT imagen FROM imagenes WHERE id = @juegoId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@juegoId", juegoId);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imagen = reader.GetString(0);
                        }
                    }
                }

                connection.Close();
            }

            return imagen;
        }

        public async Task<bool> VerificarSiTieneJuego(int usuarioId, int juegoId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM usuarios_juegos WHERE usuario_id = @usuarioId AND juego_id = @juegoId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@usuarioId", usuarioId);
                    command.Parameters.AddWithValue("@juegoId", juegoId);
                    int existe = Convert.ToInt32(command.ExecuteScalar());
                    return existe > 0;
                }
            }
        }


        public async Task AgregarJuegoAUsuarioAsync(int usuarioId, int juegoId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();

                // Obtener la imagen correspondiente del juego
                string imagenJuego = ObtenerImagenPorId(juegoId);

                // Insertar el juego en la tabla intermedia
                string queryInsert = "INSERT INTO usuarios_juegos (usuario_id, juego_id, horas, imagen) VALUES (@usuarioId, @juegoId, 0, @imagen)";
                using (SQLiteCommand insertCommand = new SQLiteCommand(queryInsert, connection))
                {
                    insertCommand.Parameters.AddWithValue("@usuarioId", usuarioId);
                    insertCommand.Parameters.AddWithValue("@juegoId", juegoId);
                    insertCommand.Parameters.AddWithValue("@imagen", imagenJuego); // Imagen asociada al juego
                    insertCommand.ExecuteNonQuery();
                }

                connection.Close();
            }
        }




        public void InsertarUsuario(string correo, string nickname, string username, string password, string imagen)
        {
            Debug.WriteLine($"Insertando usuario: {nickname}");
            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();

                // Check if the email already exists
                string checkQuery = "SELECT COUNT(*) FROM usuarios WHERE correo = @correo";
                using (SQLiteCommand checkCommand = new SQLiteCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@correo", correo);
                    long count = (long)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        Debug.WriteLine($"El correo {correo} ya está registrado.");
                        return;
                    }
                }

                // Creamos la consulta de inserción
                string query = "INSERT INTO usuarios (correo, nickname, username, password, imagen, saldo) " +
                               "VALUES (@correo, @nickname, @username, @password, @imagen, 10);";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    // Usamos parámetros para evitar inyecciones SQL
                    command.Parameters.AddWithValue("@correo", correo);
                    command.Parameters.AddWithValue("@nickname", nickname);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@imagen", imagen);

                    // Ejecutamos el comando
                    command.ExecuteNonQuery();
                }

                connection.Close();
                Debug.WriteLine($"Usuario {nickname} insertado correctamente.");
            }
        }

        private void CrearTablaUsuario()
        {
            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();

                // Creamos la consulta y la ejecutamos
                string query = "CREATE TABLE IF NOT EXISTS usuarios (" +
                                "idUser INTEGER PRIMARY KEY AUTOINCREMENT," +
                                "correo TEXT NOT NULL UNIQUE, " +
                                "nickname TEXT, " +
                                "username TEXT, " +
                                "password TEXT NOT NULL, " +
                                "imagen TEXT, " +
                                "saldo INTEGER); ";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

        }

        private void CrearTablaJuegos()
        {
            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();

                // Creamos la consulta y la ejecutamos
                string query = "CREATE TABLE IF NOT EXISTS juegos (" +
                                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                "titulo TEXT NOT NULL UNIQUE," +
                                "precio INTEGER," +
                                "descripcion TEXT); ";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private void CrearTablaUsuariosJuegos()
        {
            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();

                // Creamos la consulta y la ejecutamos
                string query = "CREATE TABLE IF NOT EXISTS usuarios_juegos (" +
                                "usuario_id INTEGER," +
                                "juego_id INTEGER, " +
                                "horas INTEGER, " +
                                "imagen TEXT, " +
                                "PRIMARY KEY(usuario_id, juego_id), " +
                                "FOREIGN KEY(usuario_id) REFERENCES usuarios(idUser), " +
                                "FOREIGN KEY(juego_id) REFERENCES juegos(id)); ";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private void CrearTablaRoles()
        {
            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();
                string query = "CREATE TABLE IF NOT EXISTS roles (" +
                               "id_rol INTEGER PRIMARY KEY AUTOINCREMENT, " +
                               "nombre TEXT NOT NULL" +
                               ");";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        private void CrearTablaUsuariosRoles()
        {
            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();
                string query = "CREATE TABLE IF NOT EXISTS usuarios_roles (" +
                               "id_usuario INTEGER, " +
                               "id_rol INTEGER, " +
                               "PRIMARY KEY(id_usuario, id_rol), " +
                               "FOREIGN KEY(id_usuario) REFERENCES usuarios(idUser), " +
                               "FOREIGN KEY(id_rol) REFERENCES roles(id_rol)" +
                               ");";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public String sacarConnection()
        {
            string rutaDirectorioApp = System.AppContext.BaseDirectory;

            DirectoryInfo directorioApp = new DirectoryInfo(rutaDirectorioApp);

            // El objeto directorio será ahora:
            // D:\Datos\proyectos_DI2425\ud04part01ExempleSQLite\SQLite03
            directorioApp = directorioApp.Parent.Parent.Parent.Parent.Parent.Parent;

            // Creamos la ruta completa del fichero de la base de datos
            // En mi ejemplo:
            // D:\Datos\proyectos_DI2425\ud04part01ExempleSQLite\SQLite03\empresa.db
            string databasePath = Path.Combine(directorioApp.FullName, "empresa.db");

            // Creamos la cadena de conexión 
            string connectionString = $"Data Source={databasePath};Version=3;";

            return connectionString;
        }

        public void ejecutarQuery(string query)
        {
            using (SQLiteConnection connection = new SQLiteConnection(sacarConnection()))
            {
                connection.Open();

                try
                {
                    // Creamos la consulta y la ejecutamos
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        // Ejecuta el comando SQL
                        command.ExecuteNonQuery();
                    }
                } catch (Exception ex) {
                    Debug.WriteLine(ex.Message);
                }

                connection.Close();
            }
        }

        public string ObtenerUsuarioDeCorreo(string correo)
        {
            if (string.IsNullOrEmpty(correo))
            {
                return string.Empty;
            }

            // Se divide el correo en dos partes, antes y después del '@'
            var usuario = correo.Split('@')[0];

            return usuario;
        }
    }
}
