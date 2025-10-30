using System;

namespace ApplicationCore.Domain.EN
{
    public class Resena
    {
        public virtual long Id { get; set; }
        public virtual int Puntuacion { get; set; }
        public virtual string? Comentario { get; set; }
        public virtual DateTime FechaPublicacion { get; set; }

        public virtual Usuario Usuario { get; set; } = null!;
        public virtual Pelicula Pelicula { get; set; } = null!;
        public virtual Reporte? Reporte { get; set; }
    }
}
