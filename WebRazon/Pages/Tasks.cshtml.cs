using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace WebRazon.Pages
{
    public class TasksModel : PageModel
    {
        private readonly ILogger<TasksModel> _logger;

        public TasksModel(ILogger<TasksModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // Esta página redirige al índice principal que contiene la interfaz de tareas
            Response.Redirect("/Index");
        }
    }
}