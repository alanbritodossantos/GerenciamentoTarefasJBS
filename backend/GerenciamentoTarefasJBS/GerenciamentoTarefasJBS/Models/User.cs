using System.Collections.Generic;

namespace GerenciamentoTarefasJBS.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        //inicializando Tasks com uma nova lista vazia.Isso garante que, se não recebermos nenhuma lista de tarefas
        //na criação de um usuário, Tasks será apenas uma lista vazia
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
