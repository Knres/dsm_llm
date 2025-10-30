using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using System.Collections.Generic;

namespace ApplicationCore.Domain.CEN
{
    public class UsuarioCEN
    {
        private readonly IUsuarioRepository _repo;
        private readonly IUnitOfWork _uow;

        public UsuarioCEN(IUsuarioRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public Usuario Create(string nombre, string email, string contrasena, bool modoBlancoYNegro = false, string? fotoPerfil = null, string? biografia = null)
        {
            var e = new Usuario
            {
                Nombre = nombre,
                Email = email,
                Contrasena = contrasena,
                ModoBlancoYNegro = modoBlancoYNegro,
                FotoPerfil = fotoPerfil,
                Biografia = biografia
            };
            _repo.New(e);
            _uow.SaveChanges();
            return e;
        }

        public void Modify(Usuario usuario)
        {
            _repo.Modify(usuario);
            _uow.SaveChanges();
        }

        public void Destroy(Usuario usuario)
        {
            _repo.Destroy(usuario);
            _uow.SaveChanges();
        }

        public IList<Usuario> ReadAll() => _repo.DameTodos();
        public Usuario? ReadById(long id) => _repo.DamePorOID(id);
        public IList<Usuario> ReadFilter(string filtro) => _repo.ReadFilter(filtro);
    }
}
