using AlumniaNotes.models;

namespace AlumniaNotes.repositories
{
    public interface IUsuarioRepository
    {
        bool ValidateCredentials(string nombreUsuario, string contrasena);
        Usuario? GetByNombreUsuario(string nombreUsuario);
    }
} 