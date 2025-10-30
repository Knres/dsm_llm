using ApplicationCore.Domain.Enums;
using System;

namespace ApplicationCore.Domain.EN
{
    public class Reporte
    {
        public virtual long Id { get; set; }
        public virtual string Motivo { get; set; } = null!;
        public virtual EstadoReporte Estado { get; set; }
        public virtual DateTime Fecha { get; set; }

        public virtual Usuario Usuario { get; set; } = null!;
        public virtual Administrador? Administrador { get; set; }
        public virtual Resena? Resena { get; set; }
    }
}
