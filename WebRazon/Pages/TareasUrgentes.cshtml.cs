using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebRazon.Pages
{
    public class TareasUrgentesModel : PageModel
    {
        private readonly ILogger<TareasUrgentesModel> _logger;

        public TareasUrgentesModel(ILogger<TareasUrgentesModel> logger)
        {
            _logger = logger;
        }

        public List<TaskItem> UrgentTasks { get; private set; }

        [BindProperty]
        public TaskItem NewTask { get; set; }

        public void OnGet()
        {
            // Inicializar con tareas urgentes de ejemplo
            UrgentTasks = new List<TaskItem>
            {
                new TaskItem { 
                    Id = 3, 
                    Title = "Corregir error en módulo de inicio de sesión", 
                    Description = "Solucionar los problemas reportados en el inicio de sesión", 
                    DueDate = DateTime.Now.AddDays(1), 
                    Status = "Urgente" 
                },
                new TaskItem { 
                    Id = 6, 
                    Title = "Presentación para cliente", 
                    Description = "Finalizar la presentación para la reunión con el cliente", 
                    DueDate = DateTime.Now.AddDays(1), 
                    Status = "Urgente" 
                },
                new TaskItem { 
                    Id = 8, 
                    Title = "Resolver bug crítico en producción", 
                    Description = "Hay un error que impide que los usuarios accedan al sistema", 
                    DueDate = DateTime.Now.AddHours(4), 
                    Status = "Urgente" 
                }
            };
        }

        public IActionResult OnPost()
        {
            // Esto normalmente guardaría la tarea urgente en una base de datos
            // Para esta implementación de solo UI, simplemente redirigimos de vuelta a la página
            return RedirectToPage();
        }
    }
}