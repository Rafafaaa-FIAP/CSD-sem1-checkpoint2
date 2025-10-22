using Microsoft.Data.SqlClient;

namespace SistemaLoja.Lab12_ConexaoSQLServer;

public class ProdutoRepository
{
    // EXERCÍCIO 1: Listar todos os produtos
    public void ListarTodosProdutos()
    {
        // TODO: Implemente a listagem de produtos
        // Dica: Use ExecuteReader e while(reader.Read())
            
        string sql = "SELECT * FROM Produtos";
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // TODO: Execute o comando e leia os resultados
                // Mostre: Id, Nome, Preco, Estoque

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Console.WriteLine($"ID: {Convert.ToInt32(dr["ID"])} - Nome: {Convert.ToString(dr["Nome"])} - Preco: {Convert.ToDecimal(dr["Preco"])} - Estoque: {Convert.ToInt32(dr["Estoque"])}");
                }
                dr.Close();
            }
        }
    }

    // EXERCÍCIO 2: Inserir novo produto
    public void InserirProduto(Produto produto)
    {
        // TODO: Implemente a inserção de produto
        // IMPORTANTE: Use parâmetros para evitar SQL Injection!
            
        string sql = "INSERT INTO Produtos (Nome, Preco, Estoque, CategoriaId) " +
                     "VALUES (@Nome, @Preco, @Estoque, @CategoriaId)";
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // TODO: Adicione os parâmetros
                cmd.Parameters.AddWithValue("@Nome", produto.Nome);
                // TODO: Complete com os outros parâmetros
                cmd.Parameters.AddWithValue("@Preco", produto.Preco);
                cmd.Parameters.AddWithValue("@Estoque", produto.Estoque);
                cmd.Parameters.AddWithValue("@CategoriaId", produto.CategoriaId);

                // TODO: Execute o comando
                cmd.ExecuteNonQuery();

                Console.WriteLine("Produto inserido com sucesso!");
            }
        }
    }

    // EXERCÍCIO 3: Atualizar produto
    public void AtualizarProduto(Produto produto)
    {
        // TODO: Implemente a atualização de produto
        // Dica: UPDATE Produtos SET ... WHERE Id = @Id
            
        string sql = "UPDATE Produtos SET " +
                     "Nome = @Nome, " +
                     "Preco = @Preco, " +
                     "Estoque = @Estoque " +
                     "WHERE Id = @Id";
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            // TODO: Complete a implementação
            conn.Open();

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Nome", produto.Nome);
                cmd.Parameters.AddWithValue("@Preco", produto.Preco);
                cmd.Parameters.AddWithValue("@Estoque", produto.Estoque);
                cmd.Parameters.AddWithValue("@Id", produto.Id);

                cmd.ExecuteNonQuery();

                Console.WriteLine("Produto atualizado com sucesso!");
            }
        }
    }

    // EXERCÍCIO 4: Deletar produto
    public void DeletarProduto(int id)
    {
        // TODO: Implemente a exclusão de produto
        // ATENÇÃO: Verifique se não há pedidos vinculados!
            
        string sql = "SELECT * FROM PedidoItens WHERE ProdutoId = @Id"; // Complete o SQL

        Boolean fl_tem_pedidos = false;

        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                SqlDataReader dr = cmd.ExecuteReader();
                fl_tem_pedidos = dr.HasRows;
                dr.Close();
            }
        }

        if (fl_tem_pedidos)
        {
            Console.WriteLine("Há pedidos vinculados com esse produto!");
        }
        else
        {
            sql = "DELETE FROM Produtos WHERE Id = @Id";

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Produto deletado com sucesso!");
                }
            }
        }
    }

    // EXERCÍCIO 5: Buscar produto por ID
    public Produto BuscarPorId(int id)
    {
        // TODO: Implemente a busca por ID
        // Retorne um objeto Produto ou null se não encontrar
            
        string sql = "SELECT * FROM Produtos WHERE Id = @Id";
        Produto produto = null;
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                    
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // TODO: Preencha o objeto produto com os dados
                        produto = new Produto
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            // TODO: Complete as outras propriedades
                            Nome = Convert.ToString(reader["Nome"]),
                            Preco = Convert.ToDecimal(reader["Preco"]),
                            Estoque = Convert.ToInt32(reader["Estoque"]),
                        };
                    }
                }
            }
        }
            
        return produto;
    }

    // EXERCÍCIO 6: Listar produtos por categoria
    public void ListarProdutosPorCategoria(int categoriaId)
    {
        // TODO: Implemente a listagem filtrada por categoria
        // Dica: Faça um JOIN com a tabela Categorias para mostrar o nome
            
        string sql = @" SELECT p.*, c.Nome as NomeCategoria 
                        FROM Produtos p
                        INNER JOIN Categorias c ON p.CategoriaId = c.Id
                        WHERE p.CategoriaId = @CategoriaId ";

        // TODO: Complete a implementação
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@CategoriaId", categoriaId);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Int32 Id = Convert.ToInt32(dr["Id"]);
                    String Nome = Convert.ToString(dr["Nome"]);
                    Decimal Preco = Convert.ToDecimal(dr["Preco"]);
                    Int32 Estoque = Convert.ToInt32(dr["Estoque"]);
                    String NomeCategoria = Convert.ToString(dr["NomeCategoria"]);

                    Console.WriteLine($"Id: {Id} - Nome: {Nome} - Preco: {Preco.ToString("N2")} - Estoque: {Estoque} - NomeCategoria: {NomeCategoria} - NomeCategoria: {NomeCategoria}");
                }
                dr.Close();
            }
        }
    }

    // DESAFIO 1: Buscar produtos com estoque baixo
    public void ListarProdutosEstoqueBaixo(int quantidadeMinima)
    {
        // TODO: Liste produtos com estoque menor que quantidadeMinima
        // Mostre um alerta visual para chamar atenção

        string sql = @" SELECT p.*, c.Nome as NomeCategoria 
                        FROM Produtos p
                        INNER JOIN Categorias c ON p.CategoriaId = c.Id
                        WHERE p.Estoque < @QuantidadeMinima ";

        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@QuantidadeMinima", quantidadeMinima);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Int32 Id = Convert.ToInt32(dr["Id"]);
                    String Nome = Convert.ToString(dr["Nome"]);
                    Decimal Preco = Convert.ToDecimal(dr["Preco"]);
                    Int32 Estoque = Convert.ToInt32(dr["Estoque"]);
                    String NomeCategoria = Convert.ToString(dr["NomeCategoria"]);

                    Console.WriteLine($"Id: {Id} - Nome: {Nome} - Preco: {Preco.ToString("N2")} - Estoque: {Estoque} - NomeCategoria: {NomeCategoria} - NomeCategoria: {NomeCategoria}");
                }
                dr.Close();
            }
        }
    }

    // DESAFIO 2: Buscar produtos por nome (LIKE)
    public void BuscarProdutosPorNome(string termoBusca)
    {
        // TODO: Implemente busca com LIKE
        // Dica: Use '%' + termoBusca + '%'

        string sql = @" SELECT p.*, c.Nome as NomeCategoria 
                        FROM Produtos p
                        INNER JOIN Categorias c ON p.CategoriaId = c.Id
                        WHERE p.Nome LIKE @TermoBusca ";

        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TermoBusca", "%" + termoBusca + "%");

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Int32 Id = Convert.ToInt32(dr["Id"]);
                    String Nome = Convert.ToString(dr["Nome"]);
                    Decimal Preco = Convert.ToDecimal(dr["Preco"]);
                    Int32 Estoque = Convert.ToInt32(dr["Estoque"]);
                    String NomeCategoria = Convert.ToString(dr["NomeCategoria"]);

                    Console.WriteLine($"Id: {Id} - Nome: {Nome} - Preco: {Preco.ToString("N2")} - Estoque: {Estoque} - NomeCategoria: {NomeCategoria} - NomeCategoria: {NomeCategoria}");
                }
                dr.Close();
            }
        }
    }
}