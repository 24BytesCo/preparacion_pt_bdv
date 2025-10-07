namespace preparacion_pt_bdv.Token
{
    // Define el contrato para obtener la sesión del usuario actual.
    public interface IUsuarioSesion
    {
        // Obtiene el identificador del usuario de la sesión activa.
        string? ObtenerUsuarioSesion();
    }
}