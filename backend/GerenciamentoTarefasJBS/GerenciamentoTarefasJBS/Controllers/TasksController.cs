using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GerenciamentoTarefasJBS.Models;
using GerenciamentoTarefasJBS.Data;
using System.Linq;
using Task = GerenciamentoTarefasJBS.Models.Task;

namespace GerenciamentoTarefasJBS.Controllers
{
    // Definimos que esta classe é um controlador de API
    [ApiController]
    // Definimos a rota que será usada para acessar este controlador
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        // Esta é uma instância do nosso contexto do banco de dados, que nos permite interagir com o banco
        private readonly ApplicationDbContext _context;

        // O construtor recebe o contexto do banco de dados por injeção de dependência
        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Este método retorna todas as tarefas do banco de dados
        // Ele responde a solicitações GET para a rota /tasks
        [HttpGet]
        public ActionResult<List<Task>> GetTasks()
        {
            return _context.Tasks.ToList();
        }

        // Este método retorna uma tarefa específica com base no ID fornecido
        // Ele responde a solicitações GET para a rota /tasks/{id}
        [HttpGet("{id}")]
        public ActionResult<Task> GetTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                // Se a tarefa não foi encontrada, retornamos um código de status 404
                return NotFound();
            }
            // Se a tarefa foi encontrada, a retornamos
            return task;
        }
    }
}
