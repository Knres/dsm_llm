using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using System.Collections.Generic;
using System;

namespace ApplicationCore.Domain.CEN
{
    public class ResenaCEN
    {
        private readonly IResenaRepository _repo;
        private readonly IUnitOfWork _uow;

        public ResenaCEN(IResenaRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public Resena Create(int puntuacion, DateTime fechaPublicacion, long usuarioId, long peliculaId, string? comentario = null)
        {
            var r = new Resena
            {
                Puntuacion = puntuacion,
                Comentario = comentario,
                FechaPublicacion = fechaPublicacion
            };
            // Note: association properties (Usuario, Pelicula) should be set by repository / CP which has access to repos for the referenced entities.
            _repo.New(r);
            _uow.SaveChanges();
            return r;
        }

        public void Modify(Resena resena)
        {
            _repo.Modify(resena);
            _uow.SaveChanges();
        }

        public void Destroy(Resena resena)
        {
            _repo.Destroy(resena);
            _uow.SaveChanges();
        }

        public IList<Resena> ReadAll() => _repo.DameTodos();
        public Resena? ReadById(long id) => _repo.DamePorOID(id);
        public IList<Resena> ReadFilter(string filtro) => _repo.ReadFilter(filtro);
    }
}
