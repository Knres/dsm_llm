using System;

namespace ApplicationCore.Domain.EN
{
    public class Notificacion
    {
        public virtual long Id { get; set; }
        public virtual string Mensaje { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual Enums.tipoNotificacion Tipo { get; set; }
        public virtual long? IdOrigen { get; set; }

        public virtual Usuario Destinatario { get; set; }
    }
}
