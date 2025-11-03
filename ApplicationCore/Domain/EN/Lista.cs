namespace ApplicationCore.Domain.EN
{
    public class Lista
    {
        public virtual long Id { get; set; }
    public virtual string Nombre { get; set; } = null!;
        public virtual Enums.tipoLista Tipo { get; set; }

        public virtual System.Collections.Generic.IList<Pelicula> Peliculas { get; set; } = new System.Collections.Generic.List<Pelicula>();
    public virtual Usuario Creador { get; set; } = null!;
    }
}
