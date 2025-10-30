using System.Collections.Generic;
using System;

namespace ApplicationCore.Domain.EN
{
    public class Pelicula
    {
        public virtual long Id { get; set; }
        public virtual string Titulo { get; set; } = null!;
        public virtual string? TituloOriginal { get; set; }
        public virtual long? Ano { get; set; }
        public virtual long? Duracion { get; set; }
        public virtual string? Pais { get; set; }
        public virtual string? Director { get; set; }
        public virtual string? Genero { get; set; }
        public virtual string? Sinopsis { get; set; }
        public virtual decimal? ValoracionMedia { get; set; }

        public virtual IList<Resena> Resenas { get; set; } = new List<Resena>();
        public virtual IList<Lista> Listas { get; set; } = new List<Lista>();
        public virtual Administrador? Administrador { get; set; }
    }
}
