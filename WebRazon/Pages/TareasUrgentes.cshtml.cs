using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using WebRazon.Models;

namespace WebRazon.Pages
{
    public class TareasUrgentesModel : PageModel
    {
        private readonly ILogger<TareasUrgentesModel> _logger;

        public TareasUrgentesModel(ILogger<TareasUrgentesModel> logger)
        {
            _logger = logger;
        }

        public List<Tarea> TareasUrgentes { get; private set; }

        [BindProperty]
        public Tarea NuevaTarea { get; set; }

        public void OnGet()
        {
            // Inicializar con tareas urgentes de ejemplo
            TareasUrgentes = new List<Tarea>
            {
                new Tarea { 
                    IdTarea = 3, 
                    NombreTarea = "Corregir error en m�dulo de inicio de sesi�n", 
                    FechaVencimiento = DateTime.Now.AddDays(1), 
                    Estado = "Urgente" 
                },
                new Tarea { 
                    IdTarea = 6, 
                    NombreTarea = "Presentaci�n para cliente", 
                    FechaVencimiento = DateTime.Now.AddDays(1), 
                    Estado = "Urgente" 
                },
                new Tarea { 
                    IdTarea = 8, 
                    NombreTarea = "Resolver bug cr�tico en producci�n", 
                    FechaVencimiento = DateTime.Now.AddHours(4), 
                    Estado = "Urgente" 
                }
            };
        }

        public IActionResult OnPost()
        {
            // Esto normalmente guardar�a la tarea urgente en una base de datos
            // Para esta implementaci�n de solo UI, simplemente redirigimos de vuelta a la p�gina
            return RedirectToPage();
        }
    }
}