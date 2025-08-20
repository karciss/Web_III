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
            // Obtener todas las tareas completadas del singleton est�tico en IndexModel
            var todasLasTareas = ObtenerTodasLasTareas();
            TareasCompletadas = todasLasTareas.Where(t => t.Estado == "Completada").ToList();
            
            // Si no hay tareas desde el IndexModel, usar datos de ejemplo
            if (TareasCompletadas.Count == 0)
            {
                TareasCompletadas = new List<Tarea>
                {
                    new Tarea
                    {
                        IdTarea = 7,
                        NombreTarea = "Revisi�n de dise�o UI",
                        FechaVencimiento = DateTime.Now.AddDays(-2),
                        Estado = "Completada"
                    }
                };
            }
        }

        // M�todo auxiliar para obtener todas las tareas de IndexModel
        private List<Tarea> ObtenerTodasLasTareas()
        {
            // Usar reflexi�n para acceder al campo est�tico privado _tareas de IndexModel
            // Esto es un enfoque temporal, en una aplicaci�n real usar�amos un servicio compartido o repositorio
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