# TP07 - Red Social

Proyecto realizado a partir del TP05 de Login y Registro.

## Tecnologias
- ASP.NET MVC / C#
- SQL Server
- Dapper
- Session
- JavaScript
- Fetch

## Antes de ejecutar
1. Ejecutar `SQL_TP07.sql` en SQL Server Management Studio.
2. Verificar la cadena de conexion en `Models/BD.cs`.
3. Ejecutar el proyecto con `dotnet run` o F5.

## Funcionalidades
- Registro, login y logout.
- Session para identificar al usuario.
- Crear publicaciones.
- Mostrar 10 publicaciones inicialmente.
- Me gusta / quitar Me gusta mediante Fetch.
- Comentarios mediante Fetch.
- Ver 10 publicaciones mas mediante Fetch y OFFSET/FETCH.
