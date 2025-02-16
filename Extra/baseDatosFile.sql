--
-- Archivo generado con SQLiteStudio v3.4.16 el do. feb. 16 01:42:58 2025
--
-- Codificación de texto usada: System
--

CREATE DATABASE IF NOT EXISTS empresa;

-- Tabla: juegos --
CREATE TABLE IF NOT EXISTS juegos (
	 id INTEGER PRIMARY KEY AUTO_INCREMENT, 
     titulo TEXT NOT NULL UNIQUE, 
     precio INTEGER, 
     descripcion TEXT, 
     imagen TEXT
 );
 
-- Datos de ejemplo (juegos) --
INSERT INTO juegos (id, titulo, precio, descripcion, imagen) VALUES 
(1, 'League of legends', 20, 'League of Legens es un juego de arena de batalla multijugador en línea (MOBA).', 'juegolol.jpg');

-- Tabla: roles --
CREATE TABLE IF NOT EXISTS roles (
	 id_rol INTEGER PRIMARY KEY AUTO_INCREMENT, 
     nombre TEXT NOT NULL);

-- Tabla: usuarios --
CREATE TABLE IF NOT EXISTS usuarios (
	 idUser INTEGER PRIMARY KEY AUTO_INCREMENT,
     correo TEXT NOT NULL UNIQUE, 
     nickname TEXT, 
     username TEXT, 
     password TEXT NOT NULL, 
     imagen TEXT, 
     saldo INTEGER
 );
 
-- Datos de pruebas (usuarios) --
/*
INSERT INTO usuarios (idUser, correo, nickname, username, password, imagen, saldo) VALUES (1, 'maek0spam@gmail.com', 'Maek0s', 'marcos', '123', 'avatarlogoosu.png', 0);
INSERT INTO usuarios (idUser, correo, nickname, username, password, imagen, saldo) VALUES (2, 'maek0spam2@gmail.com', 'maek0spam2', 'maek0spam2', '123', 'avatarlogopowercode.png', 10);
INSERT INTO usuarios (idUser, correo, nickname, username, password, imagen, saldo) VALUES (3, 'maek0szaho@gmail.com', 'maek0szaho', 'maek0szaho', '12354332', 'avatarlogopowercode.png', 10);
*/

-- Tabla: usuarios_juegos --
CREATE TABLE IF NOT EXISTS usuarios_juegos (
	usuario_id INTEGER,
    juego_id INTEGER, 
    horas INTEGER, 
    imagen TEXT, 
    PRIMARY KEY(usuario_id, juego_id), 
    FOREIGN KEY(usuario_id) REFERENCES usuarios(idUser), 
    FOREIGN KEY(juego_id) REFERENCES juegos(id)
);

-- Datos de pruebas --
/*
INSERT INTO usuarios_juegos (usuario_id, juego_id, horas, imagen) VALUES (1, 1, 120, 'logolol.png');
*/

-- Tabla: usuarios_roles
CREATE TABLE IF NOT EXISTS usuarios_roles (
    id_usuario INTEGER NOT NULL,
    id_rol INTEGER NOT NULL,
    PRIMARY KEY (id_usuario, id_rol),
    FOREIGN KEY (id_usuario) REFERENCES usuarios(idUser) ON DELETE CASCADE,
    FOREIGN KEY (id_rol) REFERENCES roles(id) ON DELETE CASCADE
);
