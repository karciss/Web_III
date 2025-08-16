using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebRazon.Pages
{
    public class TareasEnProgresoModel : PageModel
    {
        private readonly ILogger<TareasEnProgresoModel> _logger;

        public TareasEnProgresoModel(ILogger<TareasEnProgresoModel> logger)
        {
            _logger = logger;
        }

        public List<TaskItem> InProgressTasks { get; private set; }

        [BindProperty]
        public TaskItem NewTask { get; set; }

        public void OnGet()
        {
            // Inicializar con tareas en progreso de ejemplo
            InProgressTasks = new List<TaskItem>
            {
                new TaskItem { 
                    Id = 2, 
                    Title = "Revisar documentaci�n", 
                    Description = "Revisar y actualizar la documentaci�n de la API", 
                    DueDate = DateTime.Now.AddDays(5), 
                    Status = "En Progreso" 
                },
                new TaskItem { 
                    Id = 5, 
                    Title = "Reuni�n de planificaci�n", 
                    Description = "Preparar materiales para la reuni�n de planificaci�n", 
                    DueDate = DateTime.Now.AddDays(2), 
                    Status = "En Progreso" 
                }
            };
        }

        public IActionResult OnPost()
        {
            // Esto normalmente guardar�a la tarea en una base de datos
            // Para esta implementaci�n de solo UI, simplemente redirigimos de vuelta a la p�gina
            return RedirectToPage();
        }
    }
}