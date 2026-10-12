using System;

namespace GerenciadorProjetos
{
    public class Tarefa
    {
        public const string ABERTA = "Aberta";
        public const string FECHADA = "Fechada";
        public const string CANCELADA = "Cancelada";

        private int id;
        private string titulo;
        private string descricao;
        private int prioridade;          // 1=alta, 2=média, 3=baixa
        private string status;
        private DateTime dataCriacao;
        private DateTime dataConclusao;  // DateTime.MinValue = sem conclusão

        // Usado nas buscas: o id identifica a tarefa dentro do projeto.
        public Tarefa(int id) : this(id, "", "", 2) { }

        public Tarefa(int id, string titulo, string descricao, int prioridade)
        {
            this.id = id;
            this.titulo = titulo;
            this.descricao = descricao;
            this.prioridade = prioridade;
            this.status = ABERTA;
            this.dataCriacao = DateTime.Now;
            this.dataConclusao = DateTime.MinValue;
        }

        public int Id { get { return id; } set { id = value; } }
        public string Titulo { get { return titulo; } set { titulo = value; } }
        public string Descricao { get { return descricao; } set { descricao = value; } }
        public int Prioridade { get { return prioridade; } set { prioridade = value; } }
        public string Status { get { return status; } set { status = value; } }
        public DateTime DataCriacao { get { return dataCriacao; } set { dataCriacao = value; } }
        public DateTime DataConclusao { get { return dataConclusao; } set { dataConclusao = value; } }

        public string PrioridadeTexto
        {
            get
            {
                switch (prioridade)
                {
                    case 1: return "Alta";
                    case 2: return "Média";
                    case 3: return "Baixa";
                    default: return "?";
                }
            }
        }

        public void concluir()
        {
            status = FECHADA;
            dataConclusao = DateTime.Now;
        }

        public void cancelar()
        {
            status = CANCELADA;
            dataConclusao = DateTime.MinValue;
        }

        public void reabrir()
        {
            status = ABERTA;
            dataConclusao = DateTime.MinValue;
        }

        public override string ToString()
        {
            string concl = dataConclusao == DateTime.MinValue ? "-" : dataConclusao.ToString("dd/MM/yyyy HH:mm");
            return string.Format("[{0}] {1} | Prioridade: {2} | Status: {3} | Criada: {4:dd/MM/yyyy HH:mm} | Conclusão: {5}\n      {6}",
                id, titulo, PrioridadeTexto, status, dataCriacao, concl, descricao);
        }

        public override bool Equals(object obj)
        {
            Tarefa outra = obj as Tarefa;
            return outra != null && id == outra.id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}
