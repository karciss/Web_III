using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebRazon2.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Linq;
using System;

namespace WebRazon2.Pages
{
    public class EnProcesoModel : PageModel
    {
        private readonly ILogger<EnProcesoModel> _logger;

        public List<Tarea> Tareas { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanoPagina { get; set; } = 5;

        public EnProcesoModel(ILogger<EnProcesoModel> logger)
        {
            _logger = logger;
            Tareas = new List<Tarea>();
        }

        public void OnGet(int pagina = 1)
        {
            // Ruta al archivo JSON
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "tareas.json");

            // Leer el JSON y deserializarlo
            var jsonContent = System.IO.File.ReadAllText(jsonFilePath);
            var todasLasTareas = JsonSerializer.Deserialize<List<Tarea>>(jsonContent, 
                new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });

            if (todasLasTareas == null)
            {
                _logger.LogWarning("No se pudieron cargar tareas del archivo JSON");
                return;
            }

            // Filtrar solo las tareas en proceso
            var tareasEnProceso = todasLasTareas
                .Where(t => t.estado?.ToLower() == "en proceso" || t.estado?.ToLower() == "en curso")
                .ToList();

            // Lógica de paginación
            PaginaActual = pagina < 1 ? 1 : pagina;
            TotalPaginas = (int)Math.Ceiling(tareasEnProceso.Count / (double)TamanoPagina);
            
            // Asegurarse de que la página actual no exceda el total de páginas
            if (PaginaActual > TotalPaginas && TotalPaginas > 0)
            {
                PaginaActual = TotalPaginas;
            }

            // Obtener solo las tareas para la página actual
            Tareas = tareasEnProceso
                .Skip((PaginaActual - 1) * TamanoPagina)
                .Take(TamanoPagina)
                .ToList();
                
            _logger.LogInformation($"Página {PaginaActual} de {TotalPaginas} cargada con {Tareas.Count} tareas en proceso");
        }
    }
}