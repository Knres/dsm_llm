using System.Collections.Generic;

namespace ApplicationCore.Domain.EN
{
    public class Pelicula
    {
        public virtual long Id { get; set; }
        public virtual string Titulo { get; set; }
        public virtual string TituloOriginal { get; set; }
        public virtual long? Anio { get; set; }
        public virtual long? Duracion { get; set; }
        public virtual string Pais { get; set; }
        public virtual string Director { get; set; }
        public virtual string Genero { get; set; }
        public virtual string Sinopsis { get; set; }
        public virtual decimal? ValoracionMedia { get; set; }

        public virtual IList<Resena> Reseñas { get; set; } = new List<Resena>();
    }
}
