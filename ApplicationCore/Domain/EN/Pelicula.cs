using System.Collections.Generic;

namespace ApplicationCore.Domain.EN
{
    public class Pelicula
    {
        public virtual long Id { get; set; }
    public virtual string Titulo { get; set; } = null!;
    public virtual string TituloOriginal { get; set; } = null!;
        public virtual long? Anio { get; set; }
        public virtual long? Duracion { get; set; }
    public virtual string Pais { get; set; } = null!;
    public virtual string Director { get; set; } = null!;
    public virtual string Genero { get; set; } = null!;
    public virtual string Sinopsis { get; set; } = null!;
        public virtual decimal? ValoracionMedia { get; set; }

        public virtual IList<Resenya> Resenyas { get; set; } = new List<Resenya>();
    }
}
