using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Domain.EN;
using ApplicationCore.Domain.Repositories;
using ApplicationCore.Domain.CEN;

namespace ApplicationCore.Domain.CP
{
    public class ManageMetricasCP
    {
        private readonly MetricaCEN _metricaCEN;
        private readonly PeliculaCEN _peliculaCEN;
        private readonly ResenaCEN _resenaCEN;
        private readonly ListaCEN _listaCEN;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMetricaRepository _metricaRepository;
        private readonly IPeliculaRepository _peliculaRepository;
        private readonly IResenaRepository _resenaRepository;
        private readonly IListaRepository _listaRepository;

        public ManageMetricasCP(
            MetricaCEN metricaCEN,
            PeliculaCEN peliculaCEN,
            ResenaCEN resenaCEN,
            ListaCEN listaCEN,
            IUnitOfWork unitOfWork,
            IMetricaRepository metricaRepository,
            IPeliculaRepository peliculaRepository,
            IResenaRepository resenaRepository,
            IListaRepository listaRepository)
        {
            _metricaCEN = metricaCEN;
            _peliculaCEN = peliculaCEN;
            _resenaCEN = resenaCEN;
            _listaCEN = listaCEN;
            _unitOfWork = unitOfWork;
            _metricaRepository = metricaRepository;
            _peliculaRepository = peliculaRepository;
            _resenaRepository = resenaRepository;
            _listaRepository = listaRepository;
        }

    public virtual void ActualizarMetricasPelicula(long peliculaId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var pelicula = _peliculaRepository.ReadById(peliculaId);
                if (pelicula == null)
                    throw new Exception($"Película {peliculaId} no encontrada");

                var resenas = _resenaRepository.ReadAll().Where(r => r.Pelicula.Id == peliculaId).ToList();
                var listas = _listaRepository.ReadAll().Where(l => l.Peliculas.Any(p => p.Id == peliculaId)).ToList();

                // Calcular métricas
                var metrica = _metricaRepository.ReadAll().FirstOrDefault(m => m.Pelicula.Id == peliculaId) 
                             ?? new Metrica { Pelicula = pelicula };

                // Actualizar valoración media
                if (resenas.Any())
                {
                    metrica.ValoracionMedia = (double)resenas.Average(r => r.Valoracion);
                }

                // Actualizar número de reseñas
                metrica.NumeroResenas = resenas.Count;

                // Actualizar número de apariciones en listas
                metrica.AparicionesEnListas = listas.Count;

                // Calcular popularidad (fórmula personalizable)
                metrica.Popularidad = CalcularPopularidad(
                    metrica.ValoracionMedia,
                    metrica.NumeroResenas,
                    metrica.AparicionesEnListas
                );

                // Guardar o actualizar métricas
                if (metrica.Id == 0)
                    _metricaRepository.New(metrica);
                else
                    _metricaRepository.Modify(metrica);

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

    public virtual void ActualizarMetricasTodasPeliculas()
        {
            var peliculas = _peliculaRepository.ReadAll();
            foreach (var pelicula in peliculas)
            {
                ActualizarMetricasPelicula(pelicula.Id);
            }
        }

    public virtual IEnumerable<Pelicula> ObtenerPeliculasMasPopulares(int cantidad = 10)
        {
            try
            {
                var metricas = _metricaRepository.ReadAll()
                    .OrderByDescending(m => m.Popularidad)
                    .Take(cantidad)
                    .ToList();

                return metricas.Select(m => m.Pelicula);
            }
            catch
            {
                throw;
            }
        }

        private double CalcularPopularidad(double valoracionMedia, int numeroResenas, int aparicionesEnListas)
        {
            // Fórmula personalizable para calcular la popularidad
            // Aquí un ejemplo que considera todos los factores con diferentes pesos
            const double PESO_VALORACION = 0.4;
            const double PESO_RESENAS = 0.35;
            const double PESO_LISTAS = 0.25;

            // Normalizar valoración media (0-10)
            double valoracionNormalizada = valoracionMedia / 10.0;

            // Normalizar número de reseñas (logarítmico para suavizar grandes diferencias)
            double resenasNormalizadas = numeroResenas > 0 ? Math.Log10(numeroResenas) / 3.0 : 0;
            // El divisor 3.0 asume que 1000 reseñas (log10 = 3) sería el máximo "normal"
            resenasNormalizadas = Math.Min(resenasNormalizadas, 1.0);

            // Normalizar apariciones en listas (similar a reseñas)
            double listasNormalizadas = aparicionesEnListas > 0 ? Math.Log10(aparicionesEnListas) / 2.0 : 0;
            // El divisor 2.0 asume que 100 listas (log10 = 2) sería el máximo "normal"
            listasNormalizadas = Math.Min(listasNormalizadas, 1.0);

            // Calcular popularidad combinada (resultado entre 0 y 1)
            return (valoracionNormalizada * PESO_VALORACION) +
                   (resenasNormalizadas * PESO_RESENAS) +
                   (listasNormalizadas * PESO_LISTAS);
        }
    }
}