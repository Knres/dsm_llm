using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using System.Collections.Generic;
using ApplicationCore.Domain.Enums;
using System;

namespace ApplicationCore.Domain.CEN
{
    public class ReporteCEN
    {
        private readonly IReporteRepository _repo;
        private readonly IUnitOfWork _uow;

        public ReporteCEN(IReporteRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public Reporte Create(string motivo, EstadoReporte estado, DateTime fecha, long usuarioId)
        {
            var r = new Reporte
            {
                Motivo = motivo,
                Estado = estado,
                Fecha = fecha
            };
            _repo.New(r);
            _uow.SaveChanges();
            return r;
        }

        public void Modify(Reporte reporte)
        {
            _repo.Modify(reporte);
            _uow.SaveChanges();
        }

        public void Destroy(Reporte reporte)
        {
            _repo.Destroy(reporte);
            _uow.SaveChanges();
        }

        public IList<Reporte> ReadAll() => _repo.DameTodos();
        public Reporte? ReadById(long id) => _repo.DamePorOID(id);
        public IList<Reporte> ReadFilter(string filtro) => _repo.ReadFilter(filtro);
    }
}
