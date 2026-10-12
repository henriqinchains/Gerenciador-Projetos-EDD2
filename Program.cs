using System;
using System.Collections.Generic;

namespace GerenciadorProjetos
{
    class Program
    {
        static Projetos projetos = new Projetos();

        static void Main(string[] args)
        {
            int opcao;
            do
            {
                Console.WriteLine();
                Console.WriteLine("===== GERENCIADOR DE PROJETOS =====");
                Console.WriteLine(" 1. Adicionar projeto");
                Console.WriteLine(" 2. Pesquisar projeto (tarefas por status e totais)");
                Console.WriteLine(" 3. Remover projeto (apenas se sem tarefas)");
                Console.WriteLine(" 4. Adicionar tarefa em projeto");
                Console.WriteLine(" 5. Concluir tarefa");
                Console.WriteLine(" 6. Cancelar tarefa");
                Console.WriteLine(" 7. Reabrir tarefa");
                Console.WriteLine(" 8. Listar tarefas de um projeto");
                Console.WriteLine(" 9. Filtrar tarefas em um projeto");
                Console.WriteLine("10. Filtrar tarefas em todos os projetos");
                Console.WriteLine("11. Resumo geral");
                Console.WriteLine(" 0. Sair");
                opcao = LerInt("Opção: ");

                switch (opcao)
                {
                    case 0: Console.WriteLine("Encerrando..."); break;
                    case 1: AdicionarProjeto(); break;
                    case 2: PesquisarProjeto(); break;
                    case 3: RemoverProjeto(); break;
                    case 4: AdicionarTarefa(); break;
                    case 5: MudarStatus(Tarefa.FECHADA); break;
                    case 6: MudarStatus(Tarefa.CANCELADA); break;
                    case 7: MudarStatus(Tarefa.ABERTA); break;
                    case 8: ListarTarefas(); break;
                    case 9: FiltrarEmProjeto(); break;
                    case 10: FiltrarEmTodos(); break;
                    case 11: ResumoGeral(); break;
                    default: Console.WriteLine("Opção inválida."); break;
                }
            } while (opcao != 0);
        }

        // ---------- Entrada de dados ----------
        static int LerInt(string msg)
        {
            int v;
            Console.Write(msg);
            while (!int.TryParse(Console.ReadLine(), out v))
                Console.Write("Valor inválido. " + msg);
            return v;
        }

        static int LerIntEntre(string msg, int min, int max)
        {
            int v = LerInt(msg);
            while (v < min || v > max)
            {
                Console.WriteLine("Informe um valor entre " + min + " e " + max + ".");
                v = LerInt(msg);
            }
            return v;
        }

        static string LerTexto(string msg)
        {
            Console.Write(msg);
            return Console.ReadLine().Trim();
        }

        static Projeto BuscarProjeto()
        {
            int id = LerInt("ID do projeto: ");
            Projeto p = projetos.buscar(new Projeto(id));
            if (p == null) Console.WriteLine("Projeto não encontrado.");
            return p;
        }

        static Tarefa BuscarTarefa(Projeto p)
        {
            int id = LerInt("ID da tarefa: ");
            Tarefa t = p.buscarTarefa(new Tarefa(id));
            if (t == null) Console.WriteLine("Tarefa não encontrada neste projeto.");
            return t;
        }

        static void Mostrar(List<Tarefa> lista)
        {
            if (lista.Count == 0)
                Console.WriteLine("  (nenhuma tarefa)");
            foreach (Tarefa t in lista)
                Console.WriteLine("  " + t);
        }

        static string EscolherStatus()
        {
            Console.WriteLine("1. Aberta   2. Fechada   3. Cancelada");
            switch (LerIntEntre("Status: ", 1, 3))
            {
                case 1: return Tarefa.ABERTA;
                case 2: return Tarefa.FECHADA;
                default: return Tarefa.CANCELADA;
            }
        }

        // ---------- Opções ----------
        static void AdicionarProjeto()
        {
            int id = LerInt("ID do projeto: ");
            string nome = LerTexto("Nome: ");
            Console.WriteLine(projetos.adicionar(new Projeto(id, nome))
                ? "Projeto adicionado."
                : "Já existe um projeto com esse ID.");
        }

        static void PesquisarProjeto()
        {
            Projeto p = BuscarProjeto();
            if (p == null) return;

            Console.WriteLine("Projeto [" + p.Id + "] " + p.Nome);
            foreach (string s in new string[] { Tarefa.ABERTA, Tarefa.FECHADA, Tarefa.CANCELADA })
            {
                Console.WriteLine("--- " + s + "s ---");
                Mostrar(p.tarefasPorStatus(s));
            }
            Console.WriteLine("Totais -> Abertas: " + p.totalAbertas()
                + " | Fechadas: " + p.totalFechadas()
                + " | Canceladas: " + p.totalCanceladas());
        }

        static void RemoverProjeto()
        {
            Projeto p = BuscarProjeto();
            if (p == null) return;

            if (p.Tarefas.Count > 0)
            {
                Console.WriteLine("Não é possível remover: o projeto possui tarefas.");
                return;
            }
            projetos.remover(p);
            Console.WriteLine("Projeto removido.");
        }

        static void AdicionarTarefa()
        {
            Projeto p = BuscarProjeto();
            if (p == null) return;

            int id = LerInt("ID da tarefa: ");
            if (p.buscarTarefa(new Tarefa(id)) != null)
            {
                Console.WriteLine("Já existe uma tarefa com esse ID neste projeto.");
                return;
            }
            string titulo = LerTexto("Título: ");
            string descricao = LerTexto("Descrição: ");
            int prioridade = LerIntEntre("Prioridade (1=alta, 2=média, 3=baixa): ", 1, 3);

            p.adicionarTarefa(new Tarefa(id, titulo, descricao, prioridade));
            Console.WriteLine("Tarefa adicionada.");
        }

        // Concluir (Fechada), cancelar (Cancelada) ou reabrir (Aberta).
        static void MudarStatus(string novoStatus)
        {
            Projeto p = BuscarProjeto();
            if (p == null) return;
            Tarefa t = BuscarTarefa(p);
            if (t == null) return;

            if (novoStatus == Tarefa.ABERTA)
            {
                if (t.Status == Tarefa.ABERTA) { Console.WriteLine("A tarefa já está aberta."); return; }
                t.reabrir();
                Console.WriteLine("Tarefa reaberta.");
                return;
            }

            if (t.Status != Tarefa.ABERTA)
            {
                Console.WriteLine("Só é possível alterar tarefas abertas (status atual: " + t.Status + ").");
                return;
            }

            if (novoStatus == Tarefa.FECHADA)
            {
                t.concluir();
                Console.WriteLine("Tarefa concluída.");
            }
            else
            {
                t.cancelar();
                Console.WriteLine("Tarefa cancelada.");
            }
        }

        static void ListarTarefas()
        {
            Projeto p = BuscarProjeto();
            if (p == null) return;

            Console.WriteLine("Tarefas do projeto [" + p.Id + "] " + p.Nome + ":");
            Mostrar(p.Tarefas);
        }

        // Pergunta o tipo de filtro e devolve as tarefas do projeto filtradas.
        static List<Tarefa> Filtrar(Projeto p)
        {
            Console.WriteLine("1. Por status   2. Por prioridade");
            if (LerIntEntre("Filtro: ", 1, 2) == 1)
                return p.tarefasPorStatus(EscolherStatus());
            return p.tarefasPorPrioridade(LerIntEntre("Prioridade (1=alta, 2=média, 3=baixa): ", 1, 3));
        }

        static void FiltrarEmProjeto()
        {
            Projeto p = BuscarProjeto();
            if (p == null) return;
            Mostrar(Filtrar(p));
        }

        static void FiltrarEmTodos()
        {
            Console.WriteLine("1. Por status   2. Por prioridade");
            bool porStatus = LerIntEntre("Filtro: ", 1, 2) == 1;
            string status = null;
            int prioridade = 0;

            if (porStatus) status = EscolherStatus();
            else prioridade = LerIntEntre("Prioridade (1=alta, 2=média, 3=baixa): ", 1, 3);

            foreach (Projeto p in projetos.listar())
            {
                List<Tarefa> r = porStatus ? p.tarefasPorStatus(status) : p.tarefasPorPrioridade(prioridade);
                Console.WriteLine("Projeto [" + p.Id + "] " + p.Nome);
                Mostrar(r);
            }
            if (projetos.listar().Count == 0)
                Console.WriteLine("Nenhum projeto cadastrado.");
        }

        static void ResumoGeral()
        {
            int abertas = 0, fechadas = 0, canceladas = 0;
            List<Projeto> lista = projetos.listar();

            foreach (Projeto p in lista)
            {
                abertas += p.totalAbertas();
                fechadas += p.totalFechadas();
                canceladas += p.totalCanceladas();
            }

            Console.WriteLine("Projetos: " + lista.Count);
            Console.WriteLine("Tarefas abertas: " + abertas);
            Console.WriteLine("Tarefas fechadas: " + fechadas);
            Console.WriteLine("Tarefas canceladas: " + canceladas);

            // % de concluídas em relação às que foram abertas (abertas + fechadas).
            int baseCalculo = abertas + fechadas;
            if (baseCalculo == 0)
                Console.WriteLine("% concluídas: não há tarefas abertas/fechadas.");
            else
                Console.WriteLine("% concluídas: " + ((double)fechadas / baseCalculo * 100).ToString("F2") + "%");
        }
    }
}
