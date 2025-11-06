using System.Collections.Generic;
using ApplicationCore.Domain.CEN;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CP
{
    // Ejemplo de caso de proceso (orquestador simple)
    public class ManageUsuariosCP
    {
        private readonly UsuarioCEN _usuarioCEN;
        private readonly IUnitOfWork _uow;

        public ManageUsuariosCP(UsuarioCEN usuarioCEN, IUnitOfWork uow)
        {
            _usuarioCEN = usuarioCEN;
            _uow = uow;
        }

        public virtual long CrearUsuario(string nombre, string email, string contrasena, string? fotoPerfil, string? biografia, bool modoBlancoYNegro)
        {
            return _usuarioCEN.Crear(nombre, email, contrasena, fotoPerfil, biografia, modoBlancoYNegro);
        }

        public virtual IEnumerable<Usuario> ListarUsuarios() => _usuarioCEN.LeerTodos();
    }
}
