using System;

namespace WebRazon.Models
{
    public class Tarea
    {
        public int IdTarea { get; set; }
        public string NombreTarea { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Estado { get; set; }
    }
}