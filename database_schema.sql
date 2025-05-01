-- Verificar si la base de datos existe, y si no, crearla
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'AlumniaNotesDB')
BEGIN
    CREATE DATABASE AlumniaNotesDB;
END;
GO

-- Usar la base de datos
USE AlumniaNotesDB;
GO

-- Tabla Estudiantes
CREATE TABLE IF NOT EXISTS Estudiantes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(255) NOT NULL,
    Apellido VARCHAR(255) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Telefono VARCHAR(20)
);

-- Tabla Asignaturas
CREATE TABLE IF NOT EXISTS Asignaturas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(255) UNIQUE NOT NULL,
    Descripcion TEXT
);

-- Tabla Calificaciones
CREATE TABLE IF NOT EXISTS Calificaciones (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EstudianteId INT NOT NULL,
    AsignaturaId INT NOT NULL,
    Nota DECIMAL(5, 2) NOT NULL,
    Fecha DATE NOT NULL,
    FOREIGN KEY (EstudianteId) REFERENCES Estudiantes(Id),
    FOREIGN KEY (AsignaturaId) REFERENCES Asignaturas(Id)
);

-- Tabla Asistencias
CREATE TABLE IF NOT EXISTS Asistencias (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EstudianteId INT NOT NULL,
    AsignaturaId INT NOT NULL,
    Fecha DATE NOT NULL,
    Presente BIT NOT NULL,
    FOREIGN KEY (EstudianteId) REFERENCES Estudiantes(Id),
    FOREIGN KEY (AsignaturaId) REFERENCES Asignaturas(Id)
);

-- Tabla Usuarios
CREATE TABLE IF NOT EXISTS Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario VARCHAR(255) UNIQUE NOT NULL,
    Contrasena VARCHAR(255) NOT NULL,
    Rol VARCHAR(50) NOT NULL
);