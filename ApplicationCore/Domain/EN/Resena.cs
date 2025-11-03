using System;

namespace ApplicationCore.Domain.EN
{
    public class Resena
    {
        public virtual long Id { get; set; }
        public virtual long Puntuacion { get; set; }
        public virtual string Comentario { get; set; }
        public virtual DateTime FechaPublicacion { get; set; }

        // Relaciones
        public virtual Usuario Autor { get; set; }
        public virtual Pelicula Pelicula { get; set; }
    }
}
