namespace XClone.Shared.Constants
{
    public static class ResponseConstans
    {
        public const string POST_NOT_EXIST = "Post no existe";

        public const string POST_NOT_FOUND = "Post no encontrado";

        public const string USER_NOT_EXIST = "Usuario no encontrado";

        // Projects
        public const string PROJECT_NOT_EXISTS = "El proyecto no existe";


        public static string ERROR_UNEXPECTED(string traceId)
        {
            return $"Ha ocurrido un error inesperado: contacte con soporte, mencionando el siguiente código: {traceId}";
        }
    }
}
