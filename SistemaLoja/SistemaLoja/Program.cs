

using System;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using SistemaLoja.Lab12_ConexaoSQLServer;

namespace SistemaLoja
{
    // ===============================================
    // MODELOS DE DADOS
    // ===============================================

    // ===============================================
    // CLASSE DE CONEXÃO
    // ===============================================

    // ===============================================
    // REPOSITÓRIO DE PRODUTOS
    // ===============================================

    // ===============================================
    // REPOSITÓRIO DE PEDIDOS
    // ===============================================

    // ===============================================
    // CLASSE PRINCIPAL
    // ===============================================
    
    class Program
    {
        static void Main(string[] args)
        {
            // IMPORTANTE: Antes de executar, crie o banco de dados!
            // Execute o script SQL fornecido no arquivo setup.sql
            
            Console.WriteLine("=== LAB 12 - CONEXÃO SQL SERVER ===\n");
            
            var produtoRepo = new ProdutoRepository();
            var pedidoRepo = new PedidoRepository();
            
            bool continuar = true;
            
            while (continuar)
            {
                MostrarMenu();
                string opcao = Console.ReadLine();
                
                try
                {
                    switch (opcao)
                    {
                        case "1":
                            produtoRepo.ListarTodosProdutos();
                            break;
                            
                        case "2":
                            InserirNovoProduto(produtoRepo);
                            break;
                            
                        case "3":
                            AtualizarProdutoExistente(produtoRepo);
                            break;
                            
                        case "4":
                            DeletarProdutoExistente(produtoRepo);
                            break;
                            
                        case "5":
                            ListarPorCategoria(produtoRepo);
                            break;
                            
                        case "9":
                            ListarComEstoqueBaixo(produtoRepo);
                            break;
                            
                        case "10":
                            ListarPorNome(produtoRepo);
                            break;
                            
                        case "6":
                            CriarNovoPedido(pedidoRepo);
                            break;
                            
                        case "7":
                            ListarPedidosDeCliente(pedidoRepo);
                            break;
                            
                        case "8":
                            DetalhesDoPedido(pedidoRepo);
                            break;
                            
                        case "11":
                            TotalVendasPorPeriodo(pedidoRepo);
                            break;
                            
                        case "0":
                            continuar = false;
                            break;
                            
                        default:
                            Console.WriteLine("Opção inválida!");
                            break;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"\n❌ Erro SQL: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ Erro: {ex.Message}");
                }
                
                if (continuar)
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            
            Console.WriteLine("\nPrograma finalizado!");
        }

        static void MostrarMenu()
        {
            Console.WriteLine("\n╔════════════════════════════════════╗");
            Console.WriteLine("║       MENU PRINCIPAL               ║");
            Console.WriteLine("╠════════════════════════════════════╣");
            Console.WriteLine("║  PRODUTOS                          ║");
            Console.WriteLine("║  1  - Listar todos os produtos     ║");
            Console.WriteLine("║  2  - Inserir novo produto         ║");
            Console.WriteLine("║  3  - Atualizar produto            ║");
            Console.WriteLine("║  4  - Deletar produto              ║");
            Console.WriteLine("║  5  - Listar por categoria         ║");
            Console.WriteLine("║  9  - Listar com estoque baixo     ║");
            Console.WriteLine("║  10 - Buscar produto por nome      ║");
            Console.WriteLine("║                                    ║");
            Console.WriteLine("║  PEDIDOS                           ║");
            Console.WriteLine("║  6  - Criar novo pedido            ║");
            Console.WriteLine("║  7  - Listar pedidos de cliente    ║");
            Console.WriteLine("║  8  - Detalhes de um pedido        ║");
            Console.WriteLine("║  11 - Total de vendas por período  ║");
            Console.WriteLine("║                                    ║");
            Console.WriteLine("║  0  - Sair                         ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.Write("\nEscolha uma opção: ");
        }

        // TODO: Implemente os métodos auxiliares abaixo
        
        static void InserirNovoProduto(ProdutoRepository repo)
        {
            Console.WriteLine("\n=== INSERIR NOVO PRODUTO ===");
            
            // TODO: Solicite os dados do produto ao usuário
            Console.Write("Nome: ");
            string nome = Console.ReadLine();

            // TODO: Complete com Preco, Estoque, CategoriaId
            Console.Write("Preço: ");
            decimal preco;
            while (!decimal.TryParse(Console.ReadLine(), out preco))
            {
                Console.Write("Valor inválido. Digite o preço novamente: ");
            }

            Console.Write("Estoque: ");
            int estoque;
            while (!int.TryParse(Console.ReadLine(), out estoque))
            {
                Console.Write("Valor inválido. Digite o estoque novamente: ");
            }

            Console.Write("Categoria ID: ");
            int categoriaId;
            while (!int.TryParse(Console.ReadLine(), out categoriaId))
            {
                Console.Write("Valor inválido. Digite o ID da categoria novamente: ");
            }

            var produto = new Produto
            {
                // TODO: Preencha as propriedades
                Nome = nome,
                Preco = preco,
                Estoque = estoque,
                CategoriaId = categoriaId
            };
            
            repo.InserirProduto(produto);
        }

        static void AtualizarProdutoExistente(ProdutoRepository repo)
        {
            // TODO: Implemente a atualização
            Console.WriteLine("\n=== ATUALIZAR PRODUTO ===");
            
            Console.Write("ID do produto: ");
            int id = int.Parse(Console.ReadLine());

            // TODO: Busque o produto e permita alterar os dados

            Produto produto = repo.BuscarPorId(id);

            Console.Write("Nome: ");
            string nome = Console.ReadLine();

            // TODO: Complete com Preco, Estoque, CategoriaId
            Console.Write("Preço: ");
            decimal preco;
            while (!decimal.TryParse(Console.ReadLine(), out preco))
            {
                Console.Write("Valor inválido. Digite o preço novamente: ");
            }

            Console.Write("Estoque: ");
            int estoque;
            while (!int.TryParse(Console.ReadLine(), out estoque))
            {
                Console.Write("Valor inválido. Digite o estoque novamente: ");
            }

            Console.Write("Categoria ID: ");
            int categoriaId;
            while (!int.TryParse(Console.ReadLine(), out categoriaId))
            {
                Console.Write("Valor inválido. Digite o ID da categoria novamente: ");
            }

            produto.Nome = nome;
            produto.Preco = preco;
            produto.Estoque = estoque;
            produto.CategoriaId = categoriaId;

            repo.AtualizarProduto(produto);
        }

        static void DeletarProdutoExistente(ProdutoRepository repo)
        {
            // TODO: Implemente a exclusão
            Console.WriteLine("\n=== DELETAR PRODUTO ===");
            
            Console.Write("ID do produto: ");
            int id = int.Parse(Console.ReadLine());

            // TODO: Confirme antes de deletar

            Console.Write("Digite 'deletar' para confirmar: ");
            string confirm = Console.ReadLine();

            if (confirm == "deletar")
            {
                repo.DeletarProduto(id);
            }
            else
            {
                Console.Write("Produto não deletado");
            }
        }

        static void ListarPorCategoria(ProdutoRepository repo)
        {
            // TODO: Implemente
            Console.WriteLine("\n=== PRODUTOS POR CATEGORIA ===");

            Console.Write("ID da categoria: ");
            int categoriaId = int.Parse(Console.ReadLine());

            repo.ListarProdutosPorCategoria(categoriaId);
        }

        static void ListarComEstoqueBaixo(ProdutoRepository repo)
        {
            // TODO: Implemente
            Console.WriteLine("\n=== PRODUTOS COM ESTOQUE BAIXO ===");

            Console.Write("Quantidade mínima de estoque: ");
            int quantidadeMinima = int.Parse(Console.ReadLine());

            repo.ListarProdutosEstoqueBaixo(quantidadeMinima);
        }

        static void ListarPorNome(ProdutoRepository repo)
        {
            // TODO: Implemente
            Console.WriteLine("\n=== PRODUTOS POR NOME ===");

            Console.Write("Nome: ");
            string nome = Console.ReadLine();

            repo.BuscarProdutosPorNome(nome);
        }

        static void CriarNovoPedido(PedidoRepository repo)
        {
            // TODO: Implemente criação de pedido com itens
            Console.WriteLine("\n=== CRIAR NOVO PEDIDO ===");

            Console.Write("ID do Cliente: ");
            int clienteId = int.Parse(Console.ReadLine());

            List<PedidoItem> itens = new List<PedidoItem>();
            decimal valorTotal = 0;

            while (true)
            {
                Console.Write("ID do Produto (0 para finalizar): ");
                int produtoId = int.Parse(Console.ReadLine());
                if (produtoId == 0) break;

                Console.Write("Quantidade: ");
                int quantidade = int.Parse(Console.ReadLine());

                Console.Write("Preço Unitário: ");
                decimal precoUnitario = decimal.Parse(Console.ReadLine());

                decimal subtotal = quantidade * precoUnitario;
                valorTotal += subtotal;

                itens.Add(new PedidoItem
                {
                    ProdutoId = produtoId,
                    Quantidade = quantidade,
                    PrecoUnitario = precoUnitario
                });
            }

            var pedido = new Pedido
            {
                ClienteId = clienteId,
                DataPedido = DateTime.Now,
                ValorTotal = valorTotal
            };

            repo.CriarPedido(pedido, itens);
        }

        static void ListarPedidosDeCliente(PedidoRepository repo)
        {
            // TODO: Implemente
            Console.WriteLine("\n=== PEDIDOS DO CLIENTE ===");

            Console.Write("Digite o ID do Cliente: ");
            int clienteId = int.Parse(Console.ReadLine());

            repo.ListarPedidosCliente(clienteId);
        }

        static void DetalhesDoPedido(PedidoRepository repo)
        {
            // TODO: Implemente
            Console.WriteLine("\n=== DETALHES DO PEDIDO ===");

            Console.Write("Digite o ID do Pedido: ");
            int pedidoId = int.Parse(Console.ReadLine());

            repo.ObterDetalhesPedido(pedidoId);
        }
        
        static void TotalVendasPorPeriodo(PedidoRepository repo)
        {
            // TODO: Implemente
            Console.WriteLine("\n=== TOTAL DE VENDAS POR PERÍODO ===");

            Console.Write("Data inicial (dd/mm/aaaa): ");
            DateTime dataInicio = DateTime.Parse(Console.ReadLine());

            Console.Write("Data final (dd/mm/aaaa): ");
            DateTime dataFim = DateTime.Parse(Console.ReadLine());

            repo.TotalVendasPorPeriodo(dataInicio, dataFim);
        }
    }
}