using System;

namespace ApplicationCore.Domain.EN
{
    public class Resena
    {
        public virtual long Id { get; set; }
        public virtual decimal Valoracion { get; set; }
    public virtual string Comentario { get; set; } = null!;
        public virtual DateTime FechaPublicacion { get; set; }

        // Relaciones
    public virtual Usuario Autor { get; set; } = null!;
    public virtual Pelicula Pelicula { get; set; } = null!;
    }
}
