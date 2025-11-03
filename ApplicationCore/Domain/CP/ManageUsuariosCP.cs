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

        public void CrearUsuario(Usuario u)
        {
            _usuarioCEN.Crear(u);
            _uow.SaveChanges();
        }

        public IEnumerable<Usuario> ListarUsuarios() => _usuarioCEN.LeerTodos();
    }
}
