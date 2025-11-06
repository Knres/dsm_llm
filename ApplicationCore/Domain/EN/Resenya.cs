using System;

namespace ApplicationCore.Domain.EN
{
    public class Resenya
    {
        public virtual long Id { get; set; }
        public virtual long Punctuation { get; set; }
        public virtual string? Comentario { get; set; }
        public virtual DateTime Fecha { get; set; }

        // Relaciones
        public virtual Usuario Autor { get; set; } = null!;
        public virtual Pelicula Pelicula { get; set; } = null!;
    }
}
