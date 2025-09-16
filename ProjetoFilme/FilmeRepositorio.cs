using Microsoft.Data.Sqlite;

namespace ProjetoFilme;

public class FilmeRepositorio
{
    private string _databasePath;

    public FilmeRepositorio(string databasePath)
    {
        _databasePath = databasePath;
        CriarTabela();
    }

    private void CriarTabela()
    {
        var connectionString = $"Data Source={_databasePath}";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS Filmes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Titulo TEXT NOT NULL,
                    Diretor TEXT NOT NULL,
                    Lancamento TEXT NOT NULL,
                    Genero TEXT,
                    Avaliacao REAL,
                    Sinopse TEXT
                );
            ";

            tableCommand.ExecuteNonQuery();
        }
    }

    public void Adicionar(Filme novoFilme)
    {
        var connectionString = $"Data Source={_databasePath}";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO Filmes (Titulo, Diretor, Lancamento, Genero, Avaliacao, Sinopse)
                VALUES (@Titulo, @Diretor, @Lancamento, @Genero, @Avaliacao, @Sinopse)
            ";

            command.Parameters.AddWithValue("@Titulo", novoFilme.Titulo);
            command.Parameters.AddWithValue("@Diretor", novoFilme.Diretor);
            command.Parameters.AddWithValue("@Lancamento", novoFilme.Lancamento);
            command.Parameters.AddWithValue("@Genero", novoFilme.Genero ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Avaliacao", novoFilme.Avaliacao);
            command.Parameters.AddWithValue("@Sinopse", novoFilme.Sinopse ?? (object)DBNull.Value);

            command.ExecuteNonQuery();
        }
    }

    public List<Filme> ListarFilmes()
    {
        var filmes = new List<Filme>();
        var connectionString = $"Data Source={_databasePath}";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Filmes;";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var filme = new Filme
                    {
                        Id = reader.GetInt32(0),
                        Titulo = reader.GetString(1),
                        Diretor = reader.GetString(2),
                        Lancamento = reader.GetFieldValue<DateOnly>(3),
                        Genero = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Avaliacao = reader.GetDouble(5),
                        Sinopse = reader.IsDBNull(6) ? null : reader.GetString(6),
                    };
                    filmes.Add(filme);
                }
            }
        }
        return filmes;
    }

    public Filme? BuscarPorId(int id)
    {
        var connectionString = $"Data Source={_databasePath}";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Filmes WHERE Id = @Id;";
            command.Parameters.AddWithValue("@Id", id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var filme = new Filme
                    {
                        Id = reader.GetInt32(0),
                        Titulo = reader.GetString(1),
                        Diretor = reader.GetString(2),
                        Lancamento = reader.GetFieldValue<DateOnly>(3),
                        Genero = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Avaliacao = reader.GetDouble(5),
                        Sinopse = reader.IsDBNull(6) ? null : reader.GetString(6),
                    };
                    return filme;
                }
            }
        }
        return null;
    }

    public void Atualizar(Filme filmeParaAtualizar)
    {
        var connectionString = $"Data Source={_databasePath}";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE Filmes
                SET
                    Titulo = @Titulo,
                    Diretor = @Diretor,
                    Lancamento = @Lancamento,
                    Genero = @Genero,
                    Avaliacao = @Avaliacao,
                    Sinopse = @Sinopse
                WHERE Id = @Id;
            ";

            command.Parameters.AddWithValue("@Titulo", filmeParaAtualizar.Titulo);
            command.Parameters.AddWithValue("@Diretor", filmeParaAtualizar.Diretor);
            command.Parameters.AddWithValue("@Lancamento", filmeParaAtualizar.Lancamento);
            command.Parameters.AddWithValue("@Genero", filmeParaAtualizar.Genero ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Avaliacao", filmeParaAtualizar.Avaliacao);
            command.Parameters.AddWithValue("@Sinopse", filmeParaAtualizar.Sinopse ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Id", filmeParaAtualizar.Id);

            command.ExecuteNonQuery();
        }
    }

    public void Deletar(int id)
    {
        var connectionString = $"Data Source={_databasePath}";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Filmes WHERE Id = @Id;";

            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();
        }
    }
}