using ApplicationCore.Domain.Enums;
using System;

namespace ApplicationCore.Domain.EN
{
    public class Notificacion
    {
        public virtual long Id { get; set; }
        public virtual string Mensaje { get; set; } = null!;
        public virtual DateTime Fecha { get; set; }
        public virtual TipoNotificacion Tipo { get; set; }
        public virtual long? IdOrigen { get; set; }

        public virtual Usuario Usuario { get; set; } = null!;
    }
}
