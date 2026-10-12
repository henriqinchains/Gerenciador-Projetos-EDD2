using System.Collections.Generic;

namespace GerenciadorProjetos
{
    public class Projetos
    {
        private List<Projeto> itens = new List<Projeto>();

        public bool adicionar(Projeto p)
        {
            if (p == null || itens.Contains(p)) return false;
            itens.Add(p);
            return true;
        }

        public bool remover(Projeto p)
        {
            return itens.Remove(p);
        }

        public Projeto buscar(Projeto p)
        {
            int i = itens.IndexOf(p);
            return i >= 0 ? itens[i] : null;
        }

        public List<Projeto> listar()
        {
            return new List<Projeto>(itens);
        }
    }
}
