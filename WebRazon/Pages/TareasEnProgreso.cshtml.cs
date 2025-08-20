using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using WebRazon.Models;

namespace WebRazon.Pages
{
    public class TareasEnProgresoModel : PageModel
    {
        private readonly ILogger<TareasEnProgresoModel> _logger;

        public TareasEnProgresoModel(ILogger<TareasEnProgresoModel> logger)
        {
            _logger = logger;
        }

        public List<Tarea> TareasEnProgreso { get; private set; }

        [BindProperty]
        public Tarea NuevaTarea { get; set; }

        public void OnGet()
        {
            TareasEnProgreso = new List<Tarea>
            {
                new Tarea { 
                    IdTarea = 2, 
                    NombreTarea = "Revisar documentaci�n", 
                    FechaVencimiento = DateTime.Now.AddDays(5), 
                    Estado = "En Progreso" 
                },
                new Tarea { 
                    IdTarea = 5, 
                    NombreTarea = "Reuni�n de planificaci�n", 
                    FechaVencimiento = DateTime.Now.AddDays(2), 
                    Estado = "En Progreso" 
                }
            };
        }

        public IActionResult OnPost()
        {
            
            return RedirectToPage();
        }
    }
}