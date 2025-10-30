using ApplicationCore.Domain.EN;
using System.Collections.Generic;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN
{
    public class AdministradorCEN
    {
        private readonly IUnitOfWork _uow;

        public AdministradorCEN(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Administrador Create(string nombre, string email, string contrasena)
        {
            var a = new Administrador
            {
                Nombre = nombre,
                Email = email,
                Contrasena = contrasena
            };
            _uow.SaveChanges();
            return a;
        }
    }
}
