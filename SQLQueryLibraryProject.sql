USE master;
GO

-- 1. LIMPIEZA DE LA BASE DE DATOS
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'LibreriaDB')
BEGIN
    ALTER DATABASE LibreriaDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE LibreriaDB;
END
GO

CREATE DATABASE LibreriaDB;
GO

USE LibreriaDB;
GO

-- 2. ELIMINAR TABLAS SI EXISTEN (En orden de dependencia)
-- Primero borramos las que tienen FK (Hijas)
IF OBJECT_ID('VentaDetalles', 'U') IS NOT NULL DROP TABLE VentaDetalles;
IF OBJECT_ID('Ventas', 'U') IS NOT NULL DROP TABLE Ventas;
IF OBJECT_ID('Libros', 'U') IS NOT NULL DROP TABLE Libros;

-- Luego las que no dependen de nadie (Padres)
IF OBJECT_ID('Administrador', 'U') IS NOT NULL DROP TABLE Administrador;
IF OBJECT_ID('Clientes', 'U') IS NOT NULL DROP TABLE Clientes;
IF OBJECT_ID('Proveedores', 'U') IS NOT NULL DROP TABLE Proveedores;
IF OBJECT_ID('Categorias', 'U') IS NOT NULL DROP TABLE Categorias;
GO

-- 3. CREACIÓN DE TABLAS PADRE
CREATE TABLE Categorias (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100) NOT NULL,
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);

CREATE TABLE Proveedores (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20) NOT NULL,
    Correo VARCHAR(100) NOT NULL,
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);

CREATE TABLE Clientes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100) NOT NULL,
    DNI VARCHAR(100) NOT NULL,
    Correo VARCHAR(100),
    Contraseña VARCHAR(255) NOT NULL,
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);

CREATE TABLE Administrador (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100) NOT NULL,
    Correo VARCHAR(100) NOT NULL,
    Contraseña VARCHAR(255) NOT NULL
);

-- 4. CREACIÓN DE TABLAS HIJAS
CREATE TABLE Libros (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Titulo VARCHAR(150) NOT NULL,
    Autor VARCHAR(100),
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL,
    Sinopsis VARCHAR (255) NOT NULL,
    CategoriaId INT,
    ProveedorId INT,
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Libros_Categorias FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id),
    CONSTRAINT FK_Libros_Proveedores FOREIGN KEY (ProveedorId) REFERENCES Proveedores(Id)
);

CREATE TABLE Ventas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClienteId INT NOT NULL,
    FechaVenta DATETIME DEFAULT GETDATE(),
    TotalVenta DECIMAL(10,2) DEFAULT 0,
    CONSTRAINT FK_Ventas_Clientes FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
);

CREATE TABLE VentaDetalles (
    Id INT PRIMARY KEY IDENTITY(1,1),
    VentaId INT NOT NULL,
    LibroId INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_Detalle_Venta FOREIGN KEY (VentaId) REFERENCES Ventas(Id),
    CONSTRAINT FK_Detalle_Libro FOREIGN KEY (LibroId) REFERENCES Libros(Id)
);
GO

-- 5. DATOS DE PRUEBA
USE LibreriaDB;
GO

-- 1. INSERTAR CATEGORÍAS (4)
INSERT INTO Categorias (Nombre) VALUES 
('Ficción'), ('Ciencia'), ('Historia'), ('Autoayuda');

-- 2. INSERTAR PROVEEDORES (5)
INSERT INTO Proveedores (Nombre, Telefono, Correo) VALUES 
('Editorial Planeta', '2233-4455', 'contacto@planeta.com'),
('Penguin Random House', '2244-5566', 'ventas@penguin.com'),
('Distribuidora Alfa', '2255-6677', 'alfa@distri.com'),
('Librería Nacional', '2266-7788', 'info@nacional.com'),
('Ediciones Siglo XXI', '2277-8899', 'admin@sigloxxi.com');

-- 3. INSERTAR CLIENTES (10)
-- Nota: He añadido la columna Contraseña que tenías en tu tabla
INSERT INTO Clientes (Nombre, DNI, Correo, Contraseña) VALUES 
('Juan Pérez', '0801199000123', 'juan@mail.com', '$2a$11$lc4zeK40oqZ2mO7f7PJeUui9EQ18aRRidAWv8hJIMVKQdQIQZ6a/S'),
('María García', '0801199500456', 'maria@mail.com', '$2a$11$lc4zeK40oqZ2mO7f7PJeUui9EQ18aRRidAWv8hJIMVKQdQIQZ6a/S'),
('Carlos López', '0501198800789', 'carlos@mail.com', '$2a$11$lc4zeK40oqZ2mO7f7PJeUui9EQ18aRRidAWv8hJIMVKQdQIQZ6a/S'),
('Ana Martínez', '0101200000321', 'ana@mail.com', '$2a$11$lc4zeK40oqZ2mO7f7PJeUui9EQ18aRRidAWv8hJIMVKQdQIQZ6a/S'),
('Luis Rodríguez', '0801199200654', 'luis@mail.com', '$2a$11$lc4zeK40oqZ2mO7f7PJeUui9EQ18aRRidAWv8hJIMVKQdQIQZ6a/S'),
('Elena Gómez', '0301198500987', 'elena@mail.com', '$2a$11$lc4zeK40oqZ2mO7f7PJeUui9EQ18aRRidAWv8hJIMVKQdQIQZ6a/S'),
('Roberto Sosa', '0801199900111', 'roberto@mail.com', '$2a$11$lc4zeK40oqZ2mO7f7PJeUui9EQ18aRRidAWv8hJIMVKQdQIQZ6a/S'),
('Lucía Méndez', '0701199400222', 'lucia@mail.com', '$2a$11$lc4zeK40oqZ2mO7f7PJeUui9EQ18aRRidAWv8hJIMVKQdQIQZ6a/S'),
('Fernando Díaz', '0801198000333', 'fer@mail.com', '$2a$11$lc4zeK40oqZ2mO7f7PJeUui9EQ18aRRidAWv8hJIMVKQdQIQZ6a/S'),
('Sofía Castro', '0401199700444', 'sofia@mail.com', '$2a$11$lc4zeK40oqZ2mO7f7PJeUui9EQ18aRRidAWv8hJIMVKQdQIQZ6a/S');

-- 4. INSERTAR ADMINISTRADORES (5)
INSERT INTO Administrador (Nombre,Correo, Contraseña) VALUES 
('admin_general','admin_general@gmail.com', '$2a$11$7s6za0QiesyaPCRrqKcnbO5h3ZeaQANKcQzVmbQsd.nk8XulB090e'),
('soporte_tecnico','soporte@gmail.com', '$2a$11$7s6za0QiesyaPCRrqKcnbO5h3ZeaQANKcQzVmbQsd.nk8XulB090e'),
('gerente_ventas','gerente@gmial.com', '$2a$11$7s6za0QiesyaPCRrqKcnbO5h3ZeaQANKcQzVmbQsd.nk8XulB090e'),
('auditor_stock','auditor@gmail.com', '$2a$11$7s6za0QiesyaPCRrqKcnbO5h3ZeaQANKcQzVmbQsd.nk8XulB090e'),
('super_user','ThisIsProblem3bodygmail.com', '$2a$11$7s6za0QiesyaPCRrqKcnbO5h3ZeaQANKcQzVmbQsd.nk8XulB090e');


-- 5. INSERTAR LIBROS (50) 
-- Distribuidos entre las 4 categorías y 5 proveedores
INSERT INTO Libros (Titulo, Autor, Precio, Stock, Sinopsis, CategoriaId, ProveedorId) VALUES 
('Cien años de soledad', 'Gabriel García Márquez', 25.50, 20, 'La saga de la familia Buendía en el pueblo de Macondo.', 1, 1),
('Breve historia del tiempo', 'Stephen Hawking', 30.00, 15, 'Exploración de los agujeros negros y el origen del universo.', 2, 2),
('El mundo de ayer', 'Stefan Zweig', 22.00, 10, 'Memorias de un europeo sobre la pérdida de la civilización.', 3, 3),
('El poder del ahora', 'Eckhart Tolle', 18.00, 25, 'Guía espiritual para vivir en el momento presente.', 4, 4),
('Don Quijote de la Mancha', 'Miguel de Cervantes', 35.00, 12, 'Las aventuras de un caballero loco y su fiel escudero.', 1, 5),
('Cosmos', 'Carl Sagan', 28.50, 18, 'Un viaje por la historia de la ciencia y el universo.', 2, 1),
('Sapiens', 'Yuval Noah Harari', 24.00, 30, 'Breve historia de la humanidad desde los simios al futuro.', 3, 2),
('Hábitos Atómicos', 'James Clear', 20.00, 40, 'Cambios pequeños para lograr resultados extraordinarios.', 4, 3),
('Crónica de una muerte anunciada', 'G. García Márquez', 15.00, 22, 'El relato reconstruido de un asesinato por honor.', 1, 4),
('La sexta extinción', 'Elizabeth Kolbert', 26.00, 14, 'Análisis sobre el impacto humano en la biodiversidad.', 2, 5),
('Guns, Germs, and Steel', 'Jared Diamond', 29.00, 11, 'Factores ambientales que moldearon el destino de las sociedades.', 3, 1),
('Padre Rico Padre Pobre', 'Robert Kiyosaki', 19.50, 50, 'Lecciones sobre educación financiera y libertad económica.', 4, 2),
('Rayuela', 'Julio Cortázar', 23.00, 16, 'Una novela que se puede leer de múltiples maneras.', 1, 3),
('Astrophysics for People in a Hurry', 'Neil deGrasse Tyson', 17.00, 28, 'Conceptos espaciales explicados de forma rápida y sencilla.', 2, 4),
('El Imperio Romano', 'Isaac Asimov', 21.00, 13, 'Crónica detallada del ascenso y caída de Roma.', 3, 5),
('Cómo ganar amigos', 'Dale Carnegie', 16.50, 45, 'Consejos para mejorar relaciones interpersonales y liderazgo.', 4, 1),
('1984', 'George Orwell', 14.00, 35, 'Una distopía sobre el control totalitario y la vigilancia.', 1, 2),
('El gen egoísta', 'Richard Dawkins', 27.00, 10, 'Perspectiva evolucionista centrada en la genética.', 2, 3),
('La Revolución Francesa', 'Peter McPhee', 24.50, 9, 'Estudio profundo de los eventos de 1789.', 3, 4),
('Piense y hágase rico', 'Napoleon Hill', 18.50, 33, 'Filosofía de éxito personal basada en el poder mental.', 4, 5),
('La sombra del viento', 'Carlos Ruiz Zafón', 22.00, 19, 'Un misterio literario en la Barcelona de posguerra.', 1, 1),
('El origen de las especies', 'Charles Darwin', 32.00, 7, 'Fundamentos de la biología evolutiva por selección natural.', 2, 2),
('Día D', 'Antony Beevor', 30.50, 14, 'Relato crudo de la invasión aliada en Normandía.', 3, 3),
('El monje que vendió su Ferrari', 'Robin Sharma', 17.50, 26, 'Fábula espiritual sobre el éxito y el equilibrio.', 4, 4),
('Fahrenheit 451', 'Ray Bradbury', 13.50, 40, 'Sociedad futura donde los libros están prohibidos y se queman.', 1, 5),
('El universo elegante', 'Brian Greene', 31.00, 6, 'Explicación de la teoría de cuerdas y la super simetría.', 2, 1),
('Los siete pilares de la sabiduría', 'T.E. Lawrence', 33.00, 5, 'Memorias de Lawrence de Arabia durante la revuelta árabe.', 3, 2),
('Los 7 hábitos de la gente altamente efectiva', 'Stephen Covey', 21.00, 38, 'Lecciones de liderazgo y mejora de carácter.', 4, 3),
('Pedro Páramo', 'Juan Rulfo', 12.00, 21, 'Un viaje a un pueblo de fantasmas en busca de un padre.', 1, 4),
('Blink', 'Malcolm Gladwell', 20.00, 15, 'El poder de pensar sin pensar y la toma de decisiones.', 2, 5),
('Persépolis', 'Marjane Satrapi', 18.00, 12, 'Novela gráfica sobre la vida en Irán tras la revolución.', 3, 1),
('La magia del orden', 'Marie Kondo', 15.50, 42, 'Método para organizar tu hogar y transformar tu vida.', 4, 2),
('El Alquimista', 'Paulo Coelho', 14.50, 60, 'El viaje de un pastor en busca de su leyenda personal.', 1, 3),
('Homo Deus', 'Yuval Noah Harari', 26.50, 24, 'Exploración del futuro de la humanidad y la tecnología.', 2, 4),
('Línea de fuego', 'Arturo Pérez-Reverte', 24.00, 10, 'Relato sobre la Batalla del Ebro en la Guerra Civil.', 3, 5),
('Despertando al gigante interior', 'Tony Robbins', 28.00, 18, 'Estrategias para el control emocional y financiero.', 4, 1),
('La ciudad y los perros', 'Mario Vargas Llosa', 19.00, 13, 'Crítica a la disciplina militar en un colegio de Lima.', 1, 2),
('Una breve historia de casi todo', 'Bill Bryson', 27.00, 20, 'Divulgación científica sobre múltiples disciplinas.', 2, 3),
('Civilizaciones', 'Laurent Binet', 21.00, 8, 'Historia alternativa donde los Incas conquistan Europa.', 3, 4),
('Maitland: El Club de las 5 AM', 'Robin Sharma', 19.00, 29, 'Rutina matutina para elevar la productividad al máximo.', 4, 5),
('La Tregua', 'Mario Benedetti', 11.50, 17, 'Diario de un hombre cercano a la jubilación y el amor.', 1, 1),
('El orden del tiempo', 'Carlo Rovelli', 16.00, 12, 'Reflexiones físicas y filosóficas sobre el tiempo.', 2, 2),
('Stalingrado', 'Antony Beevor', 32.00, 11, 'Narración épica de la batalla más sangrienta de la historia.', 3, 3),
('Ikigai', 'Héctor García', 14.00, 48, 'Secretos de Japón para una vida larga y feliz.', 4, 4),
('Ensayo sobre la ceguera', 'José Saramago', 20.00, 14, 'Una epidemia de ceguera blanca pone a prueba la humanidad.', 1, 5),
('Gödel, Escher, Bach', 'Douglas Hofstadter', 45.00, 4, 'Un entrelazado de mentes, máquinas y matemáticas.', 2, 1),
('La Gran Guerra', 'Peter Hart', 35.00, 7, 'Historia detallada de la Primera Guerra Mundial.', 3, 2),
('Minimalismo Digital', 'Cal Newport', 22.00, 19, 'Enfoque para recuperar el enfoque en un mundo ruidoso.', 4, 3),
('Metamorfosis', 'Franz Kafka', 10.00, 50, 'Gregorio Samsa despierta convertido en un insecto.', 1, 4),
('El Problema de los Tres Cuerpos', 'Cixin Liu', 26.00, 25, 'Primer contacto de la humanidad con una civilización alienígena.', 2, 5);
GO

