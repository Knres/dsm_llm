using ApplicationCore.Domain.Enums;
using System.Collections.Generic;

namespace ApplicationCore.Domain.EN
{
    public class Lista
    {
        public virtual long Id { get; set; }
        public virtual string Nombre { get; set; } = null!;
        public virtual TipoLista Tipo { get; set; }

        public virtual Usuario Usuario { get; set; } = null!;
        public virtual IList<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
    }
}
