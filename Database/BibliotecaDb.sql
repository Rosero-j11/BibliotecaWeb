IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    CREATE TABLE [Autores] (
        [AutorId] int NOT NULL IDENTITY,
        [Nombre] nvarchar(100) NOT NULL,
        [Nacionalidad] nvarchar(60) NULL,
        CONSTRAINT [PK_Autores] PRIMARY KEY ([AutorId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    CREATE TABLE [Categorias] (
        [CategoriaId] int NOT NULL IDENTITY,
        [Nombre] nvarchar(60) NOT NULL,
        [Descripcion] nvarchar(200) NULL,
        CONSTRAINT [PK_Categorias] PRIMARY KEY ([CategoriaId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    CREATE TABLE [Socios] (
        [SocioId] int NOT NULL IDENTITY,
        [Nombre] nvarchar(100) NOT NULL,
        [Documento] nvarchar(450) NOT NULL,
        [Email] nvarchar(120) NOT NULL,
        [Telefono] nvarchar(20) NULL,
        [FechaRegistro] date NOT NULL,
        CONSTRAINT [PK_Socios] PRIMARY KEY ([SocioId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    CREATE TABLE [Libros] (
        [LibroId] int NOT NULL IDENTITY,
        [Titulo] nvarchar(150) NOT NULL,
        [ISBN] nvarchar(13) NOT NULL,
        [AnioPublicacion] int NOT NULL,
        [Ejemplares] int NOT NULL,
        [AutorId] int NOT NULL,
        [CategoriaId] int NOT NULL,
        CONSTRAINT [PK_Libros] PRIMARY KEY ([LibroId]),
        CONSTRAINT [FK_Libros_Autores_AutorId] FOREIGN KEY ([AutorId]) REFERENCES [Autores] ([AutorId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Libros_Categorias_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias] ([CategoriaId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    CREATE TABLE [Prestamos] (
        [PrestamoId] int NOT NULL IDENTITY,
        [LibroId] int NOT NULL,
        [SocioId] int NOT NULL,
        [FechaPrestamo] date NOT NULL,
        [FechaLimite] date NOT NULL,
        [FechaDevolucion] date NULL,
        CONSTRAINT [PK_Prestamos] PRIMARY KEY ([PrestamoId]),
        CONSTRAINT [FK_Prestamos_Libros_LibroId] FOREIGN KEY ([LibroId]) REFERENCES [Libros] ([LibroId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Prestamos_Socios_SocioId] FOREIGN KEY ([SocioId]) REFERENCES [Socios] ([SocioId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'AutorId', N'Nacionalidad', N'Nombre') AND [object_id] = OBJECT_ID(N'[Autores]'))
        SET IDENTITY_INSERT [Autores] ON;
    EXEC(N'INSERT INTO [Autores] ([AutorId], [Nacionalidad], [Nombre])
    VALUES (1, N''Colombiana'', N''Gabriel García Márquez''),
    (2, N''Chilena'', N''Isabel Allende''),
    (3, N''Argentina'', N''Jorge Luis Borges''),
    (4, N''Británica'', N''George Orwell''),
    (5, N''Británica'', N''J. R. R. Tolkien''),
    (6, N''Estadounidense'', N''Robert C. Martin''),
    (7, N''Colombiana'', N''Laura Restrepo'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'AutorId', N'Nacionalidad', N'Nombre') AND [object_id] = OBJECT_ID(N'[Autores]'))
        SET IDENTITY_INSERT [Autores] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CategoriaId', N'Descripcion', N'Nombre') AND [object_id] = OBJECT_ID(N'[Categorias]'))
        SET IDENTITY_INSERT [Categorias] ON;
    EXEC(N'INSERT INTO [Categorias] ([CategoriaId], [Descripcion], [Nombre])
    VALUES (1, N''Narrativa de ficción extensa'', N''Novela''),
    (2, N''Relatos cortos'', N''Cuento''),
    (3, N''Futuros posibles y distopías'', N''Ciencia ficción''),
    (4, N''Mundos imaginarios'', N''Fantasía''),
    (5, N''Programación e ingeniería de software'', N''Tecnología'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CategoriaId', N'Descripcion', N'Nombre') AND [object_id] = OBJECT_ID(N'[Categorias]'))
        SET IDENTITY_INSERT [Categorias] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SocioId', N'Documento', N'Email', N'FechaRegistro', N'Nombre', N'Telefono') AND [object_id] = OBJECT_ID(N'[Socios]'))
        SET IDENTITY_INSERT [Socios] ON;
    EXEC(N'INSERT INTO [Socios] ([SocioId], [Documento], [Email], [FechaRegistro], [Nombre], [Telefono])
    VALUES (1, N''1001234567'', N''ana.lopez@example.com'', ''2026-08-03'', N''Ana María López'', N''3001234567''),
    (2, N''1017654321'', N''carlos.gomez@example.com'', ''2026-08-20'', N''Carlos Andrés Gómez'', N''3109876543''),
    (3, N''1036987452'', N''valentina.rios@example.com'', ''2026-09-07'', N''Valentina Ríos'', NULL),
    (4, N''1152456789'', N''santiago.herrera@example.com'', ''2026-09-25'', N''Santiago Herrera'', N''3204567890''),
    (5, N''1020456123'', N''laura.martinez@example.com'', ''2026-10-10'', N''Laura Martínez'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SocioId', N'Documento', N'Email', N'FechaRegistro', N'Nombre', N'Telefono') AND [object_id] = OBJECT_ID(N'[Socios]'))
        SET IDENTITY_INSERT [Socios] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'LibroId', N'AnioPublicacion', N'AutorId', N'CategoriaId', N'Ejemplares', N'ISBN', N'Titulo') AND [object_id] = OBJECT_ID(N'[Libros]'))
        SET IDENTITY_INSERT [Libros] ON;
    EXEC(N'INSERT INTO [Libros] ([LibroId], [AnioPublicacion], [AutorId], [CategoriaId], [Ejemplares], [ISBN], [Titulo])
    VALUES (1, 1967, 1, 1, 4, N''9780307474728'', N''Cien años de soledad''),
    (2, 1985, 1, 1, 2, N''9780307387264'', N''El amor en los tiempos del cólera''),
    (3, 1981, 1, 1, 3, N''9781400034710'', N''Crónica de una muerte anunciada''),
    (4, 1982, 2, 1, 2, N''9781501117015'', N''La casa de los espíritus''),
    (5, 1944, 3, 2, 2, N''9780802130303'', N''Ficciones''),
    (6, 1949, 3, 2, 1, N''9788499089508'', N''El Aleph''),
    (7, 1949, 4, 3, 3, N''9780451524935'', N''1984''),
    (8, 1945, 4, 3, 2, N''9780451526342'', N''Rebelión en la granja''),
    (9, 1937, 5, 4, 3, N''9780547928227'', N''El hobbit''),
    (10, 1954, 5, 4, 2, N''9780547928210'', N''La comunidad del anillo''),
    (11, 2008, 6, 5, 2, N''9780132350884'', N''Código limpio''),
    (12, 2017, 6, 5, 1, N''9780134494166'', N''Arquitectura limpia''),
    (13, 2004, 7, 1, 2, N''9789587045495'', N''Delirio'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'LibroId', N'AnioPublicacion', N'AutorId', N'CategoriaId', N'Ejemplares', N'ISBN', N'Titulo') AND [object_id] = OBJECT_ID(N'[Libros]'))
        SET IDENTITY_INSERT [Libros] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PrestamoId', N'FechaDevolucion', N'FechaLimite', N'FechaPrestamo', N'LibroId', N'SocioId') AND [object_id] = OBJECT_ID(N'[Prestamos]'))
        SET IDENTITY_INSERT [Prestamos] ON;
    EXEC(N'INSERT INTO [Prestamos] ([PrestamoId], [FechaDevolucion], [FechaLimite], [FechaPrestamo], [LibroId], [SocioId])
    VALUES (1, ''2026-09-14'', ''2026-09-16'', ''2026-09-01'', 1, 1),
    (2, NULL, ''2026-10-20'', ''2026-10-05'', 7, 2),
    (3, NULL, ''2026-11-12'', ''2026-10-28'', 9, 3),
    (4, NULL, ''2026-11-17'', ''2026-11-02'', 11, 4),
    (5, NULL, ''2026-10-30'', ''2026-10-15'', 6, 1),
    (6, ''2026-10-16'', ''2026-10-16'', ''2026-10-01'', 4, 5)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PrestamoId', N'FechaDevolucion', N'FechaLimite', N'FechaPrestamo', N'LibroId', N'SocioId') AND [object_id] = OBJECT_ID(N'[Prestamos]'))
        SET IDENTITY_INSERT [Prestamos] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Categorias_Nombre] ON [Categorias] ([Nombre]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Libros_AutorId] ON [Libros] ([AutorId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Libros_CategoriaId] ON [Libros] ([CategoriaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Libros_ISBN] ON [Libros] ([ISBN]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Prestamos_LibroId] ON [Prestamos] ([LibroId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Prestamos_SocioId] ON [Prestamos] ([SocioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Socios_Documento] ON [Socios] ([Documento]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261105200516_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261105200516_InitialCreate', N'10.0.14');
END;

COMMIT;
GO

