using System.Collections.Generic;

namespace GerenciadorProjetos
{
    public class Projeto
    {
        private int id;
        private string nome;
        private List<Tarefa> tarefas;

        // Usado nas buscas: o id identifica o projeto.
        public Projeto(int id) : this(id, "") { }

        public Projeto(int id, string nome)
        {
            this.id = id;
            this.nome = nome;
            this.tarefas = new List<Tarefa>();
        }

        public int Id { get { return id; } set { id = value; } }
        public string Nome { get { return nome; } set { nome = value; } }
        public List<Tarefa> Tarefas { get { return tarefas; } }

        public void adicionarTarefa(Tarefa t)
        {
            tarefas.Add(t);
        }

        public bool removerTarefa(Tarefa t)
        {
            return tarefas.Remove(t);
        }

        public Tarefa buscarTarefa(Tarefa t)
        {
            int i = tarefas.IndexOf(t);
            return i >= 0 ? tarefas[i] : null;
        }

        public List<Tarefa> tarefasPorStatus(string s)
        {
            List<Tarefa> r = new List<Tarefa>();
            foreach (Tarefa t in tarefas)
                if (t.Status == s) r.Add(t);
            return r;
        }

        public List<Tarefa> tarefasPorPrioridade(int p)
        {
            List<Tarefa> r = new List<Tarefa>();
            foreach (Tarefa t in tarefas)
                if (t.Prioridade == p) r.Add(t);
            return r;
        }

        public int totalAbertas()
        {
            return tarefasPorStatus(Tarefa.ABERTA).Count;
        }

        public int totalFechadas()
        {
            return tarefasPorStatus(Tarefa.FECHADA).Count;
        }

        // Extra (usado no resumo geral).
        public int totalCanceladas()
        {
            return tarefasPorStatus(Tarefa.CANCELADA).Count;
        }

        public override bool Equals(object obj)
        {
            Projeto outro = obj as Projeto;
            return outro != null && id == outro.id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}
