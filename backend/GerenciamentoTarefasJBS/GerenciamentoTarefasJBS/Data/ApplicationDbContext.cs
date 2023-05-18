using GerenciamentoTarefasJBS.Models;
using Task = GerenciamentoTarefasJBS.Models.Task;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoTarefasJBS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
}
