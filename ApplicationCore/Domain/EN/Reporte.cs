using System;

namespace ApplicationCore.Domain.EN
{
    public class Reporte
    {
        public virtual long Id { get; set; }
    public virtual string Motivo { get; set; } = null!;
        public virtual Enums.estadoReporte Estado { get; set; }
        public virtual DateTime Fecha { get; set; }

    public virtual Usuario Autor { get; set; } = null!;
    public virtual Resena SobreResena { get; set; } = null!;
    }
}
