-- Crear la base de datos
CREATE DATABASE AlumniaNotes;
GO

USE AlumniaNotes;
GO

-- Crear tabla Usuarios
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario NVARCHAR(50) NOT NULL UNIQUE,
    Contrasena NVARCHAR(100) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    Apellidos NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Rol NVARCHAR(20) NOT NULL CHECK (Rol IN ('Administrador', 'Profesor', 'Alumno')),
    FechaCreacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);
GO

-- Crear tabla Alumnos
CREATE TABLE Alumnos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Apellidos NVARCHAR(50) NOT NULL,
    DNI NVARCHAR(9) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Telefono NVARCHAR(15),
    Direccion NVARCHAR(200),
    FechaNacimiento DATE,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);
GO

-- Crear tabla Asignaturas
CREATE TABLE Asignaturas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Codigo NVARCHAR(20) NOT NULL UNIQUE,
    Creditos INT NOT NULL CHECK (Creditos > 0),
    Descripcion NVARCHAR(MAX),
    FechaCreacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);
GO

-- Crear tabla Calificaciones
CREATE TABLE Calificaciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AlumnoId INT NOT NULL,
    AsignaturaId INT NOT NULL,
    Nota DECIMAL(4,2) NOT NULL CHECK (Nota >= 0 AND Nota <= 10),
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Tipo NVARCHAR(20) NOT NULL CHECK (Tipo IN ('Examen', 'Trabajo', 'Practica', 'Proyecto')),
    Comentario NVARCHAR(MAX),
    FOREIGN KEY (AlumnoId) REFERENCES Alumnos(Id),
    FOREIGN KEY (AsignaturaId) REFERENCES Asignaturas(Id)
);
GO

-- Crear tabla Asistencias
CREATE TABLE Asistencias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AlumnoId INT NOT NULL,
    AsignaturaId INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Estado NVARCHAR(20) NOT NULL CHECK (Estado IN ('Presente', 'Ausente', 'Justificado', 'Retraso')),
    Comentario NVARCHAR(MAX),
    FOREIGN KEY (AlumnoId) REFERENCES Alumnos(Id),
    FOREIGN KEY (AsignaturaId) REFERENCES Asignaturas(Id)
);
GO

-- Crear índices para mejorar el rendimiento
CREATE INDEX IX_Calificaciones_AlumnoId ON Calificaciones(AlumnoId);
CREATE INDEX IX_Calificaciones_AsignaturaId ON Calificaciones(AsignaturaId);
CREATE INDEX IX_Asistencias_AlumnoId ON Asistencias(AlumnoId);
CREATE INDEX IX_Asistencias_AsignaturaId ON Asistencias(AsignaturaId);
CREATE INDEX IX_Asistencias_Fecha ON Asistencias(Fecha);
GO

-- Insertar datos de ejemplo
-- Usuario administrador
INSERT INTO Usuarios (NombreUsuario, Contrasena, Nombre, Apellidos, Email, Rol)
VALUES ('admin', 'admin123', 'Administrador', 'Sistema', 'admin@alumnianotes.com', 'Administrador');
GO

-- Alumno de ejemplo
INSERT INTO Alumnos (Nombre, Apellidos, DNI, Email, Telefono, Direccion, FechaNacimiento)
VALUES ('Juan', 'Pérez García', '12345678A', 'juan.perez@example.com', '600123456', 'Calle Mayor 1', '2000-01-01');
GO

-- Asignatura de ejemplo
INSERT INTO Asignaturas (Nombre, Codigo, Creditos, Descripcion)
VALUES ('Programación', 'PROG-101', 6, 'Fundamentos de programación');
GO

-- Calificación de ejemplo
INSERT INTO Calificaciones (AlumnoId, AsignaturaId, Nota, Tipo, Comentario)
VALUES (1, 1, 8.5, 'Examen', 'Buen rendimiento');
GO

-- Asistencia de ejemplo
INSERT INTO Asistencias (AlumnoId, AsignaturaId, Estado, Comentario)
VALUES (1, 1, 'Presente', 'Asistencia normal');
GO 