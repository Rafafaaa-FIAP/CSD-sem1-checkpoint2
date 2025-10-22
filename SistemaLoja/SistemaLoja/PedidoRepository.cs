using Microsoft.Data.SqlClient;

namespace SistemaLoja.Lab12_ConexaoSQLServer;

public class PedidoRepository
{
    // EXERCÍCIO 7: Criar pedido com itens (transação)
    public void CriarPedido(Pedido pedido, List<PedidoItem> itens)
    {
        // TODO: Implemente criação de pedido com transação
        // 1. Inserir Pedido
        // 2. Inserir cada PedidoItem
        // 3. Atualizar estoque dos produtos
        // IMPORTANTE: Use SqlTransaction!
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            // TODO: Inicie a transação
            SqlTransaction transaction = conn.BeginTransaction();
                
            try
            {
                // TODO: 1. Inserir pedido e obter ID
                string sqlPedido = "INSERT INTO Pedidos (ClienteId, DataPedido, ValorTotal) " +
                                   "OUTPUT INSERTED.Id " +
                                   "VALUES (@ClienteId, @DataPedido, @ValorTotal)";

                int pedidoId;
                using (SqlCommand cmd = new SqlCommand(sqlPedido, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@ClienteId", pedido.ClienteId);
                    cmd.Parameters.AddWithValue("@DataPedido", pedido.DataPedido);
                    cmd.Parameters.AddWithValue("@ValorTotal", pedido.ValorTotal);

                    pedidoId = (int)cmd.ExecuteScalar();
                }

                // TODO: 2. Inserir itens do pedido

                // TODO: 3. Atualizar estoque

                string sqlItem = @"
                INSERT INTO PedidoItens (PedidoId, ProdutoId, Quantidade, PrecoUnitario)
                VALUES (@PedidoId, @ProdutoId, @Quantidade, @PrecoUnitario)";

                foreach (var item in itens)
                {
                    using (SqlCommand cmdItem = new SqlCommand(sqlItem, conn, transaction))
                    {
                        cmdItem.Parameters.AddWithValue("@PedidoId", pedidoId);
                        cmdItem.Parameters.AddWithValue("@ProdutoId", item.ProdutoId);
                        cmdItem.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                        cmdItem.Parameters.AddWithValue("@PrecoUnitario", item.PrecoUnitario);
                        cmdItem.ExecuteNonQuery();
                    }

                    string sqlEstoque = "UPDATE Produtos SET Estoque = Estoque - @Qtd WHERE Id = @ProdutoId";
                    using (SqlCommand cmdEstoque = new SqlCommand(sqlEstoque, conn, transaction))
                    {
                        cmdEstoque.Parameters.AddWithValue("@Qtd", item.Quantidade);
                        cmdEstoque.Parameters.AddWithValue("@ProdutoId", item.ProdutoId);
                        cmdEstoque.ExecuteNonQuery();
                    }
                }

                // TODO: Commit da transação
                transaction.Commit();
                Console.WriteLine("Pedido criado com sucesso!");
            }
            catch (Exception ex)
            {
                // TODO: Rollback em caso de erro
                transaction.Rollback();
                Console.WriteLine($"Erro ao criar pedido: {ex.Message}");
                throw;
            }
        }
    }

    // EXERCÍCIO 8: Listar pedidos de um cliente
    public void ListarPedidosCliente(int clienteId)
    {
        // TODO: Liste todos os pedidos de um cliente
        // Mostre: Id, Data, ValorTotal
            
        string sql = "SELECT * FROM Pedidos WHERE ClienteId = @ClienteId ORDER BY DataPedido DESC";

        // TODO: Complete a implementação

        using (SqlConnection conn = DatabaseConnection.GetConnection())
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@ClienteId", clienteId);
            conn.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\n--- Lista de Pedidos ---");

                if (!reader.HasRows)
                {
                    Console.WriteLine("Nenhum pedido encontrado para este cliente.");
                    return;
                }

                while (reader.Read())
                {
                    int id = reader.GetInt32(reader.GetOrdinal("Id"));
                    DateTime data = reader.GetDateTime(reader.GetOrdinal("DataPedido"));
                    decimal valor = reader.GetDecimal(reader.GetOrdinal("ValorTotal"));

                    Console.WriteLine($"ID: {id} | Data: {data:dd/MM/yyyy HH:mm} | Total: R$ {valor:F2}");
                }
            }
        }
    }

    // EXERCÍCIO 9: Obter detalhes completos de um pedido
    public void ObterDetalhesPedido(int pedidoId)
    {
        // TODO: Mostre o pedido com todos os itens
        // Faça JOIN com Produtos para mostrar nomes
            
        // TODO: Complete a implementação

        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();

            // 1️⃣ Buscar cabeçalho do pedido
            string sqlPedido = @"
            SELECT p.Id, p.DataPedido, p.ValorTotal, c.Nome AS NomeCliente
            FROM Pedidos p
            INNER JOIN Clientes c ON p.ClienteId = c.Id
            WHERE p.Id = @PedidoId";

            using (SqlCommand cmdPedido = new SqlCommand(sqlPedido, conn))
            {
                cmdPedido.Parameters.AddWithValue("@PedidoId", pedidoId);

                using (SqlDataReader reader = cmdPedido.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        Console.WriteLine("Pedido não encontrado.");
                        return;
                    }

                    Console.WriteLine($"\n--- Pedido #{reader["Id"]} ---");
                    Console.WriteLine($"Cliente: {reader["NomeCliente"]}");
                    Console.WriteLine($"Data: {Convert.ToDateTime(reader["DataPedido"]):dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"Valor Total: R$ {Convert.ToDecimal(reader["ValorTotal"]):F2}");
                }
            }

            // 2️⃣ Buscar os itens do pedido
            string sqlItens = @"
            SELECT 
                pi.ProdutoId,
                p.Nome AS NomeProduto,
                pi.Quantidade,
                pi.PrecoUnitario,
                (pi.Quantidade * pi.PrecoUnitario) AS Subtotal
            FROM PedidoItens pi
            INNER JOIN Produtos p ON pi.ProdutoId = p.Id
            WHERE pi.PedidoId = @PedidoId";

            using (SqlCommand cmdItens = new SqlCommand(sqlItens, conn))
            {
                cmdItens.Parameters.AddWithValue("@PedidoId", pedidoId);

                using (SqlDataReader readerItens = cmdItens.ExecuteReader())
                {
                    Console.WriteLine("\n--- Itens do Pedido ---");

                    if (!readerItens.HasRows)
                    {
                        Console.WriteLine("Nenhum item encontrado para este pedido.");
                        return;
                    }

                    while (readerItens.Read())
                    {
                        string nomeProduto = readerItens["NomeProduto"].ToString();
                        int qtd = Convert.ToInt32(readerItens["Quantidade"]);
                        decimal preco = Convert.ToDecimal(readerItens["PrecoUnitario"]);
                        decimal subtotal = Convert.ToDecimal(readerItens["Subtotal"]);

                        Console.WriteLine($"Produto: {nomeProduto,-20} | Qtd: {qtd,-3} | Preço: R$ {preco,6:F2} | Subtotal: R$ {subtotal,7:F2}");
                    }
                }
            }
        }
    }

    // DESAFIO 3: Calcular total de vendas por período
    public void TotalVendasPorPeriodo(DateTime dataInicio, DateTime dataFim)
    {
        // TODO: Calcule o total de vendas em um período
        // Use ExecuteScalar para obter a soma

        string sql = @" SELECT SUM(ValorTotal)
                        FROM Pedidos
                        WHERE DataPedido BETWEEN @DataInicio AND @DataFim ";

        using (SqlConnection conn = DatabaseConnection.GetConnection())
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@DataInicio", dataInicio);
            cmd.Parameters.AddWithValue("@DataFim", dataFim);

            conn.Open();
            object resultado = cmd.ExecuteScalar();

            decimal total = 0;
            if (resultado != DBNull.Value && resultado != null)
                total = Convert.ToDecimal(resultado);

            Console.WriteLine($"\nPeríodo: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}");
            Console.WriteLine($"Total de Vendas: R$ {total:F2}");
        }
    }
}