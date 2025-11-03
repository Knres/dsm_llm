using System;

namespace ApplicationCore.Domain.EN
{
    public class Metrica
    {
        public virtual long Id { get; set; }
        public virtual double ValoracionMedia { get; set; }
        public virtual int NumeroResenas { get; set; }
        public virtual int AparicionesEnListas { get; set; }
        public virtual double Popularidad { get; set; }
        public virtual DateTime Fecha { get; set; }

    public virtual Pelicula Pelicula { get; set; } = null!;
    }
}
