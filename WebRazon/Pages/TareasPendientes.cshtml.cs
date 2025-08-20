using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using WebRazon.Models;

namespace WebRazon.Pages
{
    public class TareasPendientesModel : PageModel
    {
        private readonly ILogger<TareasPendientesModel> _logger;

        public TareasPendientesModel(ILogger<TareasPendientesModel> logger)
        {
            _logger = logger;
        }

        public List<Tarea> TareasPendientes { get; private set; }

        [BindProperty]
        public Tarea NuevaTarea { get; set; }

        public void OnGet()
        {
            // Inicializar con tareas pendientes de ejemplo
            TareasPendientes = new List<Tarea>
            {
                new Tarea { 
                    IdTarea = 1, 
                    NombreTarea = "Completar propuesta de proyecto", 
                    FechaVencimiento = DateTime.Now.AddDays(3), 
                    Estado = "Pendiente" 
                },
                new Tarea { 
                    IdTarea = 4, 
                    NombreTarea = "Actualizar bibliotecas del proyecto", 
                    FechaVencimiento = DateTime.Now.AddDays(7), 
                    Estado = "Pendiente" 
                }
            };
        }

        public IActionResult OnPost()
        {
            // Esto normalmente guardaría la tarea pendiente en una base de datos
            // Para esta implementación de solo UI, simplemente redirigimos de vuelta a la página
            return RedirectToPage();
        }
    }
}