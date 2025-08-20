using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using WebRazon.Models;

namespace WebRazon.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private static List<Tarea> _tareas; 

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            
            if (_tareas == null)
            {
                _tareas = new List<Tarea>
                {
                    new Tarea { 
                        IdTarea = 1, 
                        NombreTarea = "Completar propuesta de proyecto", 
                        FechaVencimiento = DateTime.Now.AddDays(3), 
                        Estado = "Pendiente" 
                    },
                    new Tarea { 
                        IdTarea = 2, 
                        NombreTarea = "Revisar documentación", 
                        FechaVencimiento = DateTime.Now.AddDays(5), 
                        Estado = "En Progreso" 
                    },
                    new Tarea { 
                        IdTarea = 3, 
                        NombreTarea = "Corregir error en módulo de inicio de sesión", 
                        FechaVencimiento = DateTime.Now.AddDays(1), 
                        Estado = "Urgente" 
                    },
                    new Tarea { 
                        IdTarea = 4, 
                        NombreTarea = "Actualizar bibliotecas del proyecto", 
                        FechaVencimiento = DateTime.Now.AddDays(7), 
                        Estado = "Pendiente" 
                    },
                    new Tarea { 
                        IdTarea = 5, 
                        NombreTarea = "Reunión de planificación", 
                        FechaVencimiento = DateTime.Now.AddDays(2), 
                        Estado = "En Progreso" 
                    },
                    new Tarea { 
                        IdTarea = 6, 
                        NombreTarea = "Presentación para cliente", 
                        FechaVencimiento = DateTime.Now.AddDays(1), 
                        Estado = "Urgente" 
                    },
                    new Tarea { 
                        IdTarea = 7, 
                        NombreTarea = "Revisión de diseño UI", 
                        FechaVencimiento = DateTime.Now.AddDays(-2), 
                        Estado = "Completada" 
                    },
                };
            }
        }

        public List<Tarea> Tareas { get; private set; }

        [BindProperty(SupportsGet = true)]
        public string Filtro { get; set; }

        [BindProperty]
        public Tarea NuevaTarea { get; set; }

        public void OnGet()
        {
            if (string.IsNullOrEmpty(Filtro) || Filtro.Equals("todas", StringComparison.OrdinalIgnoreCase))
            {
                Tareas = _tareas;
            }
            else
            {
                Tareas = _tareas.Where(t => t.Estado.Equals(Filtro, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                NuevaTarea.IdTarea = _tareas.Count > 0 ? _tareas.Max(t => t.IdTarea) + 1 : 1;
                
                _tareas.Add(NuevaTarea);
                
                _logger.LogInformation($"Nueva tarea creada: {NuevaTarea.NombreTarea}");
            }
            
            return RedirectToPage();
        }

        public JsonResult OnGetTareas()
        {
            return new JsonResult(_tareas);
        }
        
        public JsonResult OnGetTareaPorId(int id)
        {
            var tarea = _tareas.FirstOrDefault(t => t.IdTarea == id);
            
            if (tarea == null)
            {
                return new JsonResult(new { success = false, message = "Tarea no encontrada" }) { StatusCode = 404 };
            }
            
            return new JsonResult(new { success = true, tarea });
        }
        
        public JsonResult OnPostAgregarTarea([FromBody] Tarea tarea)
        {
            if (string.IsNullOrEmpty(tarea.NombreTarea))
            {
                return new JsonResult(new { success = false, message = "El nombre de la tarea es obligatorio" }) { StatusCode = 400 };
            }
            
            
            tarea.IdTarea = _tareas.Count > 0 ? _tareas.Max(t => t.IdTarea) + 1 : 1;
            
            
            _tareas.Add(tarea);
            
            _logger.LogInformation($"Nueva tarea creada vía API: {tarea.NombreTarea}");
            
            return new JsonResult(new { success = true, tarea });
        }
        
        public JsonResult OnPostActualizarTarea([FromBody] Tarea tarea)
        {
            if (tarea == null || tarea.IdTarea <= 0)
            {
                return new JsonResult(new { success = false, message = "Información de tarea inválida" }) { StatusCode = 400 };
            }
            
            var tareaExistente = _tareas.FirstOrDefault(t => t.IdTarea == tarea.IdTarea);
            
            if (tareaExistente == null)
            {
                return new JsonResult(new { success = false, message = "Tarea no encontrada" }) { StatusCode = 404 };
            }
            
            
            tareaExistente.NombreTarea = tarea.NombreTarea;
            tareaExistente.FechaVencimiento = tarea.FechaVencimiento;
            tareaExistente.Estado = tarea.Estado;
            
            _logger.LogInformation($"Tarea actualizada: {tarea.NombreTarea}");
            
            return new JsonResult(new { success = true, tarea = tareaExistente });
        }
        
        public JsonResult OnPostActualizarEstadoTarea(int id, string estado)
        {
            var tarea = _tareas.FirstOrDefault(t => t.IdTarea == id);
            
            if (tarea == null)
            {
                return new JsonResult(new { success = false, message = "Tarea no encontrada" }) { StatusCode = 404 };
            }
            
            tarea.Estado = estado;
            
            return new JsonResult(new { success = true });
        }
        
        public JsonResult OnPostEliminarTarea(int id)
        {
            var tarea = _tareas.FirstOrDefault(t => t.IdTarea == id);
            
            if (tarea == null)
            {
                return new JsonResult(new { success = false, message = "Tarea no encontrada" }) { StatusCode = 404 };
            }
            
            _tareas.Remove(tarea);
            
            return new JsonResult(new { success = true });
        }
    }
}
