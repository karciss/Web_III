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
                    Title = "Corregir error en m�dulo de inicio de sesi�n", 
                    Description = "Solucionar los problemas reportados en el inicio de sesi�n", 
                    DueDate = DateTime.Now.AddDays(1), 
                    Status = "Urgente" 
                },
                new TaskItem { 
                    Id = 6, 
                    Title = "Presentaci�n para cliente", 
                    Description = "Finalizar la presentaci�n para la reuni�n con el cliente", 
                    DueDate = DateTime.Now.AddDays(1), 
                    Status = "Urgente" 
                },
                new TaskItem { 
                    Id = 8, 
                    Title = "Resolver bug cr�tico en producci�n", 
                    Description = "Hay un error que impide que los usuarios accedan al sistema", 
                    DueDate = DateTime.Now.AddHours(4), 
                    Status = "Urgente" 
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