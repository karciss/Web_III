using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace WebRazon.Pages
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
    }

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<TaskItem> Tasks { get; private set; }

        [BindProperty]
        public TaskItem NewTask { get; set; }

        public void OnGet()
        {
            // Inicializar con tareas de ejemplo - en una aplicación real, esto vendría de una base de datos
            Tasks = new List<TaskItem>
            {
                new TaskItem { 
                    Id = 1, 
                    Title = "Completar propuesta de proyecto", 
                    Description = "Escribir una propuesta detallada para el nuevo proyecto del cliente", 
                    DueDate = DateTime.Now.AddDays(3), 
                    Status = "Pendiente" 
                },
                new TaskItem { 
                    Id = 2, 
                    Title = "Revisar documentación", 
                    Description = "Revisar y actualizar la documentación de la API", 
                    DueDate = DateTime.Now.AddDays(5), 
                    Status = "En Progreso" 
                },
                new TaskItem { 
                    Id = 3, 
                    Title = "Corregir error en módulo de inicio de sesión", 
                    Description = "Solucionar los problemas reportados en el inicio de sesión", 
                    DueDate = DateTime.Now.AddDays(1), 
                    Status = "Urgente" 
                }
            };
        }

        public IActionResult OnPost()
        {
            // Esto normalmente guardaría la tarea en una base de datos
            // Para esta implementación de solo UI, simplemente redirigimos de vuelta a la página
            return RedirectToPage();
        }
    }
}
