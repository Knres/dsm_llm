using System;

namespace ApplicationCore.Domain.EN
{
    public class Notificacion
    {
        public virtual long Id { get; set; }
    public virtual string Mensaje { get; set; } = null!;
        public virtual DateTime Fecha { get; set; }
        public virtual Enums.tipoNotificacion Tipo { get; set; }
        public virtual long? IdOrigen { get; set; }
        public virtual bool Leida { get; set; }
        public virtual DateTime? FechaLeida { get; set; }

        // Nuevo atributo para representar el estado de la notificación
        public virtual Enums.estadoNotificacion EstadoNotificacion { get; set; } = Enums.estadoNotificacion.NoLeida;

    public virtual Usuario Destinatario { get; set; } = null!;
    }
}
