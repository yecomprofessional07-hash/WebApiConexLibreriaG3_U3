USE master
GO

-- Eliminando base de datos 
DROP DATABASE IF EXISTS LibreriaDB;
GO

-- Creacion de base de datos
CREATE DATABASE LibreriaDB;
GO

-- Utilizar base de datos
USE LibreriaDB
GO
-- 1. Eliminar tablas en orden inverso (primero la que tiene la llave foránea)
/*Esta parte la investigue*/

IF OBJECT_ID('Libros', 'U') IS NOT NULL DROP TABLE Libros;
IF OBJECT_ID('Categorias', 'U') IS NOT NULL DROP TABLE Categorias;
IF OBJECT_ID('Proveedores', 'U') IS NOT NULL DROP TABLE Proveedores;
IF OBJECT_ID('Clientes', 'U') IS NOT NULL DROP TABLE Clientes;
GO
-- Tabla Categorias
CREATE TABLE Categorias (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100) NOT NULL,
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);

-- Tabla Proveedores
CREATE TABLE Proveedores (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20) NOT NULL,
    Correo VARCHAR(100) NOT NULL,
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);

-- Tabla Clientes
CREATE TABLE Clientes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100) NOT NULL,
    DNI VARCHAR(100) NOT NULL,
    Correo VARCHAR(100),
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);

-- Tabla Libros
CREATE TABLE Libros (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Titulo VARCHAR(150) NOT NULL,
    Autor VARCHAR(100),
    Precio DECIMAL(10,2),
    Stock INT,
    CategoriaId INT,
    ProveedorId INT,
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id),
    FOREIGN KEY (ProveedorId) REFERENCES Proveedores(Id)
);
