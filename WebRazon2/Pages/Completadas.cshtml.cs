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
    public class CompletadasModel : PageModel
    {
        private readonly ILogger<CompletadasModel> _logger;

        public List<Tarea> Tareas { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanoPagina { get; set; } = 5;
        
        // Lista de opciones para el tamaño de página
        public List<int> OpcionesTamanoPagina { get; } = new List<int> { 5, 10, 15, 20 };

        public CompletadasModel(ILogger<CompletadasModel> logger)
        {
            _logger = logger;
            Tareas = new List<Tarea>();
        }

        public void OnGet(int pagina = 1, int tamanoPagina = 5)
        {
            if (OpcionesTamanoPagina.Contains(tamanoPagina))
            {
                TamanoPagina = tamanoPagina;
            }
            
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

            // solo las tareas completadas
            var tareasCompletadas = todasLasTareas
                .Where(t => t.estado?.ToLower() == "finalizado" || t.estado?.ToLower() == "completada")
                .ToList();

            PaginaActual = pagina < 1 ? 1 : pagina;
            TotalPaginas = (int)Math.Ceiling(tareasCompletadas.Count / (double)TamanoPagina);
            
            if (PaginaActual > TotalPaginas && TotalPaginas > 0)
            {
                PaginaActual = TotalPaginas;
            }

            // Obtener solo las tareas para la página actual
            Tareas = tareasCompletadas
                .Skip((PaginaActual - 1) * TamanoPagina)
                .Take(TamanoPagina)
                .ToList();
                
            _logger.LogInformation($"Página {PaginaActual} de {TotalPaginas} cargada con {Tareas.Count} tareas completadas. Tamaño de página: {TamanoPagina}");
        }
    }
}   