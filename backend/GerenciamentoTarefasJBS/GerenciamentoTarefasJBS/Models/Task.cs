namespace GerenciamentoTarefasJBS.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
