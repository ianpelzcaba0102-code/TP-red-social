USE master;
GO

IF DB_ID('session') IS NULL
BEGIN
    CREATE DATABASE session;
END
GO

USE session;
GO

-- Si se parte del TP05 original, Usuario ya existe sin IdUsuario.
-- Se agrega el identificador para poder relacionar las tablas nuevas.
IF OBJECT_ID('dbo.Usuario', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuario
    (
        IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
        nombre VARCHAR(50) NOT NULL,
        apellido VARCHAR(50) NOT NULL,
        nombreUsuario VARCHAR(50) NOT NULL,
        contraseña VARCHAR(50) NOT NULL
    );
END
ELSE IF COL_LENGTH('dbo.Usuario', 'IdUsuario') IS NULL
BEGIN
    ALTER TABLE dbo.Usuario ADD IdUsuario INT IDENTITY(1,1) NOT NULL;
END
GO

IF OBJECT_ID('dbo.Publicacion', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Publicacion
    (
        IdPublicacion INT IDENTITY(1,1) PRIMARY KEY,
        Imagen VARCHAR(500) NOT NULL,
        Titulo VARCHAR(150) NOT NULL,
        Descripcion VARCHAR(1000) NOT NULL,
        IdUsuario INT NOT NULL,
        FechaPublicacion DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Publicacion_Usuario FOREIGN KEY (IdUsuario) REFERENCES dbo.Usuario(IdUsuario)
    );
END
GO

IF OBJECT_ID('dbo.[Like]', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[Like]
    (
        IdPublicacion INT NOT NULL,
        IdUsuario INT NOT NULL,
        CONSTRAINT PK_Like PRIMARY KEY (IdPublicacion, IdUsuario),
        CONSTRAINT FK_Like_Publicacion FOREIGN KEY (IdPublicacion) REFERENCES dbo.Publicacion(IdPublicacion),
        CONSTRAINT FK_Like_Usuario FOREIGN KEY (IdUsuario) REFERENCES dbo.Usuario(IdUsuario)
    );
END
GO

IF OBJECT_ID('dbo.Comentario', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Comentario
    (
        IdComentario INT IDENTITY(1,1) PRIMARY KEY,
        IdPublicacion INT NOT NULL,
        IdUsuario INT NOT NULL,
        Texto VARCHAR(1000) NOT NULL,
        FechaComentario DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Comentario_Publicacion FOREIGN KEY (IdPublicacion) REFERENCES dbo.Publicacion(IdPublicacion),
        CONSTRAINT FK_Comentario_Usuario FOREIGN KEY (IdUsuario) REFERENCES dbo.Usuario(IdUsuario)
    );
END
GO
