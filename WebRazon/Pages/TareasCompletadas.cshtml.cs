using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using WebRazon.Models;

namespace WebRazon.Pages
{
    public class TareasCompletadasModel : PageModel
    {
        private readonly ILogger<TareasCompletadasModel> _logger;

        public TareasCompletadasModel(ILogger<TareasCompletadasModel> logger)
        {
            _logger = logger;
        }

        public List<Tarea> TareasCompletadas { get; private set; }

        public void OnGet()
        {
            var todasLasTareas = ObtenerTodasLasTareas();
            TareasCompletadas = todasLasTareas.Where(t => t.Estado == "Completada").ToList();
            
            if (TareasCompletadas.Count == 0)
            {
                TareasCompletadas = new List<Tarea>
                {
                    new Tarea
                    {
                        IdTarea = 7,
                        NombreTarea = "Revisión de diseño UI",
                        FechaVencimiento = DateTime.Now.AddDays(-2),
                        Estado = "Completada"
                    }
                };
            }
        }

        private List<Tarea> ObtenerTodasLasTareas()
        {
            
            var indexModelType = typeof(IndexModel);
            var allTasksField = indexModelType.GetField("_tareas", 
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Static);

            if (allTasksField != null)
            {
                return allTasksField.GetValue(null) as List<Tarea> ?? new List<Tarea>();
            }

            return new List<Tarea>();
        }
    }
}