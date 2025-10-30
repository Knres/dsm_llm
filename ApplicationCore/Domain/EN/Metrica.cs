using System;

namespace ApplicationCore.Domain.EN
{
    public class Metrica
    {
        public virtual long Id { get; set; }
        public virtual string Estadisticas { get; set; } = null!;
        public virtual DateTime Fecha { get; set; }

        public virtual Usuario Usuario { get; set; } = null!;
    }
}
