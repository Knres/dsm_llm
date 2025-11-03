using System;

namespace ApplicationCore.Domain.EN
{
    public class Metrica
    {
        public virtual long Id { get; set; }
        public virtual string Estadisticas { get; set; }
        public virtual DateTime Fecha { get; set; }

        // Relación: Metrica consulta Usuario (ejemplo)
        public virtual Usuario Usuario { get; set; }
    }
}
