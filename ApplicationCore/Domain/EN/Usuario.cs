using System.Collections.Generic;

namespace ApplicationCore.Domain.EN
{
    public class Usuario
    {
        public virtual long Id { get; set; }
        public virtual string Nombre { get; set; } = null!;
        public virtual string Email { get; set; } = null!;
        public virtual string Contrasena { get; set; } = null!;
        public virtual string? FotoPerfil { get; set; }
        public virtual string? Biografia { get; set; }
        public virtual bool ModoBlancoYNegro { get; set; }

        public virtual IList<Resena> Resenas { get; set; } = new List<Resena>();
        public virtual IList<Lista> Listas { get; set; } = new List<Lista>();
        public virtual IList<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
        public virtual IList<Reporte> Reportes { get; set; } = new List<Reporte>();
        public virtual Metrica? Metrica { get; set; }
    }
}
