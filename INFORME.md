# Informe del trabajo práctico: Red Social

## 1. Objetivo
El proyecto consiste en crear una pequeña red social con ASP.NET MVC, utilizando SQL Server como base de datos y sesiones para mantener la autenticación del usuario. La aplicación permite registrar usuarios, iniciar sesión, crear publicaciones, comentar y dar me gusta.

## 2. Tecnologías utilizadas
- ASP.NET Core MVC
- C#
- SQL Server
- Dapper    
- Session de ASP.NET Core
- HTML, CSS y JavaScript
- Fetch para peticiones AJAX

## 3. Estructura del proyecto
- `Controllers/UsuarioController.cs`: controla el registro, login y logout.
- `Controllers/PublicacionController.cs`: controla las publicaciones, likes y comentarios.
- `Models/BD.cs`: clase que encapsula la lógica de acceso a la base de datos.
- `Models/Usuario.cs`: modelo del usuario.
- `Models/Publicacion.cs`: modelo de la publicación.
- `Models/Comentario.cs`: modelo del comentario.
- `Views/`: vistas principales del sistema.
- `BD.sql`: script único para crear la base de datos y las tablas necesarias.

## 4. Base de datos
La base de datos se llama `session` y se crea desde el archivo `BD.sql`.

### Tablas principales
- `Usuario`: almacena nombre, apellido, nombre de usuario y contraseña.
- `Publicacion`: almacena la imagen, título, descripción, autor y fecha.
- `Like`: representa likes asociados a una publicación y un usuario.
- `Comentario`: almacena comentarios asociados a una publicación y a un usuario.

### Relación entre tablas
- Un usuario puede tener muchas publicaciones.
- Una publicación puede tener muchos comentarios.
- Una publicación puede tener muchos likes.
- Un usuario puede dar muchos likes y escribir muchos comentarios.

## 5. Flujo de ejecución
1. El usuario entra a la aplicación.
2. Si no tiene cuenta, puede registrarse.
3. Al registrarse, se validan los datos.
4. Si el usuario existe, no se permite duplicar la cuenta.
5. Al iniciar sesión, el sistema busca el usuario en la base de datos.
6. Si es correcto, guarda el nombre de usuario y su ID en la sesión.
7. Desde ahí, puede crear publicaciones, verlas y comentar.
8. Cada operación se valida en el backend para evitar inconsistencias.

## 6. Uso de sesiones
La sesión permite identificar al usuario autenticado en cada request. Esto es clave para saber quién está logueado y evitar que un usuario no autenticado acceda a secciones que requieren login.

En `UsuarioController` se guarda:
- `Usuario`: el nombre de usuario.
- `IdUsuario`: el identificador del usuario.

Esto permite que `PublicacionController` valide si el usuario está logueado antes de permitir acciones como crear una publicación o dar like.

## 7. Lógica de publicaciones
El sistema puede mostrar publicaciones en bloques de 10 elementos. Cuando el usuario quiere cargar más, se usa `OFFSET` y `FETCH` para traer el siguiente grupo sin recargar la página completa.

## 8. Likes y comentarios
- Los likes se manejan con una tabla `Like` que evita duplicados mediante la clave primaria compuesta `(IdPublicacion, IdUsuario)`.
- Si el usuario ya dio like, lo quita; si no, lo agrega.
- Los comentarios se insertan enlazados a una publicación y se devuelven junto con el nombre del usuario que los escribió.

## 9. JavaScript y Fetch
Se usa JavaScript con `fetch` para:
- dar/quitar like,
- comentar,
- cargar más publicaciones.
Esto permite una experiencia más fluida y evita recargar toda la vista.

## 10. Resultados del proyecto
El proyecto cumple con los objetivos principales de una red social básica:
- Registro y login.
- Manejo de sesión.
- Publicaciones.
- Likes.
- Comentarios.
- Paginación.

## 11. Conclusión
Este trabajo demuestra la integración entre Backend, Base de Datos y Frontend. La parte más importante es la conexión entre SQL Server y ASP.NET MVC, donde los datos se guardan de forma persistente y se consumen dinámicamente para construir una experiencia realista de red social.
