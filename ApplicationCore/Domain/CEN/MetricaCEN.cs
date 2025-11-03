using System;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;

namespace ApplicationCore.Domain.CEN
{
    public class MetricaCEN
    {
        protected readonly IMetricaRepository _metricaRepository;
        protected readonly IUnitOfWork _unitOfWork;

        public MetricaCEN(IMetricaRepository metricaRepository, IUnitOfWork unitOfWork)
        {
            _metricaRepository = metricaRepository;
            _unitOfWork = unitOfWork;
        }

        // Nota: Según el modelo de dominio, Metrica solo tiene métodos de lectura
    public virtual Metrica ObtenerPorId(long id)
        {
            return _metricaRepository.ReadById(id);
        }

    public virtual IList<Metrica> ObtenerTodas()
        {
            return _metricaRepository.ReadAll();
        }
    }
}