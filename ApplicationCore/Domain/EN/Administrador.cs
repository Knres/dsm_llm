using System.Collections.Generic;

namespace ApplicationCore.Domain.EN
{
    public class Administrador
    {
        public virtual long Id { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Email { get; set; }
        public virtual string Contrasena { get; set; }

        public virtual IList<Reporte> ReportesResueltos { get; set; } = new List<Reporte>();
        public virtual IList<Pelicula> PeliculasPublicadas { get; set; } = new List<Pelicula>();
    }
}
