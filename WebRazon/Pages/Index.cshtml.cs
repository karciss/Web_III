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
            // Inicializar con tareas de ejemplo - en una aplicaci�n real, esto vendr�a de una base de datos
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
                    Title = "Revisar documentaci�n", 
                    Description = "Revisar y actualizar la documentaci�n de la API", 
                    DueDate = DateTime.Now.AddDays(5), 
                    Status = "En Progreso" 
                },
                new TaskItem { 
                    Id = 3, 
                    Title = "Corregir error en m�dulo de inicio de sesi�n", 
                    Description = "Solucionar los problemas reportados en el inicio de sesi�n", 
                    DueDate = DateTime.Now.AddDays(1), 
                    Status = "Urgente" 
                },
                new TaskItem { 
                    Id = 4, 
                    Title = "Actualizar bibliotecas del proyecto", 
                    Description = "Actualizar todas las bibliotecas a las �ltimas versiones", 
                    DueDate = DateTime.Now.AddDays(7), 
                    Status = "Pendiente" 
                },
                new TaskItem { 
                    Id = 5, 
                    Title = "Reuni�n de planificaci�n", 
                    Description = "Preparar materiales para la reuni�n de planificaci�n", 
                    DueDate = DateTime.Now.AddDays(2), 
                    Status = "En Progreso" 
                },
                new TaskItem { 
                    Id = 6, 
                    Title = "Presentaci�n para cliente", 
                    Description = "Finalizar la presentaci�n para la reuni�n con el cliente", 
                    DueDate = DateTime.Now.AddDays(1), 
                    Status = "Urgente" 
                },
                new TaskItem { 
                    Id = 7, 
                    Title = "Revisi�n de dise�o UI", 
                    Description = "Completar la revisi�n de dise�os de interfaz para la app m�vil", 
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
            // Esto normalmente guardar�a la tarea en una base de datos
            // Para esta implementaci�n de solo UI, simplemente redirigimos de vuelta a la p�gina
            return RedirectToPage();
        }
    }
}
