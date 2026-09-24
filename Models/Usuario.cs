namespace TP05_SESSIONS.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string nombreUsuario { get; set; }
        public string contraseña { get; set; }
        public string tipousuario { get; set; }

        public string ValidarLogin()
        {
            if (nombreUsuario ==null || nombreUsuario == "")
            {
                return "Complete el usuario";
            }
            if (contraseña == null || contraseña == "")
            {
                return "Complete la contraseña";
            }
            if (nombreUsuario.Length < 4)
            {
                return "El usuario debe tener como mínimo 4 caracteres";
            }
            if (contraseña.Length < 4)
            {
                return "La contraseña debe tener como mínimo 4 caracteres";
            }
            return "";
        }

        public string ValidarRegistro()
        {
            if (nombreUsuario == null || nombreUsuario == "")
            {
                return "Complete el usuario";
            }
            if (contraseña == null || contraseña == "")
            {
                return "Complete la contraseña";
            }
            if (nombre == null || nombre == "")
            {
                return "Complete el nombre";
            }

            if (apellido == null || apellido == "")
            {
                return "Complete el apellido";
            }
            for (int i = 0; i < nombre.Length; i++)
            {
                char caracter = nombre[i];
                if (!( (caracter>= 'A' && caracter<= 'Z') || (caracter >= 'a' && caracter <= 'z') || caracter == ' ' ))
                {
                    return "El nombre solo puede contener letras y espacios";
                }
            }
            for (int i = 0; i < apellido.Length; i++)
            {
                char caracter = apellido[i];
                if (!((caracter >= 'A' && caracter <= 'Z') || (caracter >= 'a' && caracter <= 'z') || caracter == ' ' ))
                {
                    return "El apellido solo puede contener letras y espacios";
                }
            }
            if (tipousuario == null || tipousuario == "")
            {
                return "Seleccione un tipo de usuario";
            }
            if (nombreUsuario.Length < 4)
            {
                return "El usuario debe tener como mínimo 4 caracteres";
            }
            if (contraseña.Length < 4)
            {
                return "La contraseña debe tener como mínimo 4 caracteres";
            }
            return "";
        }
    }
}