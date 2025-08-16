using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Ejer2.Models;

namespace Ejer2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<Models.Task> Tasks { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Tasks = new List<Models.Task>
            {
                new Models.Task
                {
                    Id = 1,
                    Name = "Completar informe mensual",
                    Description = "Finalizar el informe de ventas del mes de abril",
                    DueDate = DateTime.Now.AddDays(3),
                    Status = Models.TaskStatus.EnProgreso,
                    CreatedDate = DateTime.Now.AddDays(-5)
                },
                new Models.Task
                {
                    Id = 2,
                    Name = "Reunión con clientes",
                    Description = "Preparar presentación para la reunión con los nuevos clientes",
                    DueDate = DateTime.Now.AddDays(1),
                    Status = Models.TaskStatus.Pendiente,
                    CreatedDate = DateTime.Now.AddDays(-2)
                },
                new Models.Task
                {
                    Id = 3,
                    Name = "Actualizar sitio web",
                    Description = "Actualizar la página principal con las nuevas promociones",
                    DueDate = DateTime.Now.AddDays(-1),
                    Status = Models.TaskStatus.Completada,
                    CreatedDate = DateTime.Now.AddDays(-7)
                },
                new Models.Task
                {
                    Id = 4,
                    Name = "Revisar presupuesto",
                    Description = "Analizar el presupuesto para el próximo trimestre",
                    DueDate = DateTime.Now.AddDays(5),
                    Status = Models.TaskStatus.Pendiente,
                    CreatedDate = DateTime.Now.AddDays(-1)
                },
                new Models.Task
                {
                    Id = 5,
                    Name = "Entrenamiento del personal",
                    Description = "Organizar sesión de entrenamiento para nuevos empleados",
                    DueDate = DateTime.Now.AddDays(-2),
                    Status = Models.TaskStatus.Cancelada,
                    CreatedDate = DateTime.Now.AddDays(-10)
                }
            };
        }
    }
}
