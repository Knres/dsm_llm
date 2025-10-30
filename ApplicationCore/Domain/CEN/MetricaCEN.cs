using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using System.Collections.Generic;
using System;

namespace ApplicationCore.Domain.CEN
{
    public class MetricaCEN
    {
        private readonly IUnitOfWork _uow;

        public MetricaCEN(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Metrica Create(string estadisticas, DateTime fecha, long usuarioId)
        {
            var m = new Metrica
            {
                Estadisticas = estadisticas,
                Fecha = fecha
            };
            // Saving would require a repository; placeholder behaviour for now
            _uow.SaveChanges();
            return m;
        }
    }
}
