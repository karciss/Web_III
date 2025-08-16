using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebRazon.Pages
{
    public class TareasPendientesModel : PageModel
    {
        private readonly ILogger<TareasPendientesModel> _logger;

        public TareasPendientesModel(ILogger<TareasPendientesModel> logger)
        {
            _logger = logger;
        }

        public List<TaskItem> PendingTasks { get; private set; }

        [BindProperty]
        public TaskItem NewTask { get; set; }

        public void OnGet()
        {
            // Inicializar con tareas pendientes de ejemplo
            PendingTasks = new List<TaskItem>
            {
                new TaskItem { 
                    Id = 1, 
                    Title = "Completar propuesta de proyecto", 
                    Description = "Escribir una propuesta detallada para el nuevo proyecto del cliente", 
                    DueDate = DateTime.Now.AddDays(3), 
                    Status = "Pendiente" 
                },
                new TaskItem { 
                    Id = 4, 
                    Title = "Actualizar bibliotecas del proyecto", 
                    Description = "Actualizar todas las bibliotecas a las últimas versiones", 
                    DueDate = DateTime.Now.AddDays(7), 
                    Status = "Pendiente" 
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