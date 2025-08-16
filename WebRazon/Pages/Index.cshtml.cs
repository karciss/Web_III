using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private List<TaskItem> _allTasks;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<TaskItem> Tasks { get; private set; }

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; }

        [BindProperty]
        public TaskItem NewTask { get; set; }

        public void OnGet()
        {
            // Inicializar con tareas de ejemplo - en una aplicación real, esto vendría de una base de datos
            _allTasks = new List<TaskItem>
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
                },
                new TaskItem { 
                    Id = 4, 
                    Title = "Actualizar bibliotecas del proyecto", 
                    Description = "Actualizar todas las bibliotecas a las últimas versiones", 
                    DueDate = DateTime.Now.AddDays(7), 
                    Status = "Pendiente" 
                },
                new TaskItem { 
                    Id = 5, 
                    Title = "Reunión de planificación", 
                    Description = "Preparar materiales para la reunión de planificación", 
                    DueDate = DateTime.Now.AddDays(2), 
                    Status = "En Progreso" 
                },
                new TaskItem { 
                    Id = 6, 
                    Title = "Presentación para cliente", 
                    Description = "Finalizar la presentación para la reunión con el cliente", 
                    DueDate = DateTime.Now.AddDays(1), 
                    Status = "Urgente" 
                },
                new TaskItem { 
                    Id = 7, 
                    Title = "Revisión de diseño UI", 
                    Description = "Completar la revisión de diseños de interfaz para la app móvil", 
                    DueDate = DateTime.Now.AddDays(-2), 
                    Status = "Completada" 
                },
            };

            // Aplicar filtrado si es necesario
            if (string.IsNullOrEmpty(Filter) || Filter.Equals("todas", StringComparison.OrdinalIgnoreCase))
            {
                Tasks = _allTasks;
            }
            else
            {
                Tasks = _allTasks.Where(t => t.Status.Equals(Filter, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        public IActionResult OnPost()
        {
            // Esto normalmente guardaría la tarea en una base de datos
            // Para esta implementación de solo UI, simplemente redirigimos de vuelta a la página
            return RedirectToPage();
        }
    }
}
