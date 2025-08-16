using System;
using System.ComponentModel.DataAnnotations;

namespace Ejer2.Models
{
    public class Task
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "La fecha de vencimiento es requerida")]
        [Display(Name = "Fecha de Vencimiento")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        
        [Display(Name = "Estado")]
        public TaskStatus Status { get; set; }
        
        [Display(Name = "Fecha de Creación")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
    
    public enum TaskStatus
    {
        Pendiente,
        EnProgreso,
        Completada,
        Cancelada
    }
}