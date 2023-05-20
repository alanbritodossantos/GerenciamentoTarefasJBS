using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GerenciamentoTarefasJBS.Models;
using GerenciamentoTarefasJBS.Data;
using System.Linq;
using Task = GerenciamentoTarefasJBS.Models.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GerenciamentoTarefasJBS.Controllers
{
    [Authorize]// todas as ações na casse Tasks exijam autenticação
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
        public async Task<ActionResult<List<Task>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        // Este método retorna uma tarefa específica com base no ID fornecido
        // Ele responde a solicitações GET para a rota /tasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                // Se a tarefa não foi encontrada, retornamos um código de status 404
                return NotFound();
            }
            // Se a tarefa foi encontrada, a retornamos
            return task;
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult<Task>> PostTask(Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Task>> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return task;
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }


    }
}
