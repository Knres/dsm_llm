using System;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Enums;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN
{
    public class ReporteCEN
    {
        protected readonly IReporteRepository _reporteRepository;
        protected readonly IUnitOfWork _unitOfWork;

        public ReporteCEN(IReporteRepository reporteRepository, IUnitOfWork unitOfWork)
        {
            _reporteRepository = reporteRepository;
            _unitOfWork = unitOfWork;
        }

        public long Crear(string motivo, estadoReporte estado, DateTime fecha)
        {
            var reporte = new Reporte
            {
                Motivo = motivo,
                Estado = estado,
                Fecha = fecha
            };

            _reporteRepository.New(reporte);
            _unitOfWork.SaveChanges();
            return reporte.Id;
        }

        public void Modificar(long id, string motivo, estadoReporte estado)
        {
            var reporte = _reporteRepository.ReadById(id);
            if (reporte == null)
                throw new Exception($"Reporte {id} no encontrado");

            reporte.Motivo = motivo;
            reporte.Estado = estado;

            _reporteRepository.Modify(reporte);
            _unitOfWork.SaveChanges();
        }

        public void Eliminar(long id)
        {
            var reporte = _reporteRepository.ReadById(id);
            if (reporte == null)
                throw new Exception($"Reporte {id} no encontrado");

            _reporteRepository.Delete(reporte);
            _unitOfWork.SaveChanges();
        }

        public Reporte ObtenerPorId(long id)
        {
            return _reporteRepository.ReadById(id);
        }

        public IList<Reporte> ObtenerTodos()
        {
            return _reporteRepository.ReadAll();
        }
    }
}