using Microsoft.Data.Sqlite;
using ProjetoFilme;

namespace TestProject;

public class FilmeRepositorioTests : IDisposable
{
    private readonly string _testDatabasePath;
    private readonly FilmeRepositorio _filmeRepo;

    public FilmeRepositorioTests()
    {
        _testDatabasePath = Path.Combine(Path.GetTempPath(), $"test-filmes-{Guid.NewGuid()}.db");

        using (var conn = new SqliteConnection($"Data Source={_testDatabasePath}"))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE Filmes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Titulo TEXT NOT NULL,
                    Diretor TEXT NOT NULL,
                    Lancamento TEXT NOT NULL,
                    Genero TEXT,
                    Avaliacao REAL,
                    Sinopse TEXT
                );
            ";
            cmd.ExecuteNonQuery();
        }

        _filmeRepo = new FilmeRepositorio(_testDatabasePath);
    }

    public void Dispose()
    {
        if (File.Exists(_testDatabasePath))
        {
            File.Delete(_testDatabasePath);
        }
    }

    [Fact]
    public void Adicionar_NovoFilme_DeveSalvarNoBanco()
    {
        var novoFilme = new Filme
        {
            Titulo = "A Origem",
            Diretor = "Christopher Nolan",
            Lancamento = new DateOnly(2010, 7, 16),
            Genero = "Ficção Científica",
            Avaliacao = 5.0,
            Sinopse = "Um ladrão que rouba segredos..."
        };

        _filmeRepo.Adicionar(novoFilme);

        var filmeSalvo = _filmeRepo.BuscarPorId(1);
        Assert.NotNull(filmeSalvo);
        Assert.Equal("A Origem", filmeSalvo.Titulo);
        Assert.Equal("Christopher Nolan", filmeSalvo.Diretor);
    }

    [Fact]
    public void ListarTodos_ComFilmes_DeveRetornarListaCompleta()
    {
        _filmeRepo.Adicionar(new Filme { Titulo = "Filme 1", Diretor = "Dir 1", Lancamento = new DateOnly(2000, 1, 1) });
        _filmeRepo.Adicionar(new Filme { Titulo = "Filme 2", Diretor = "Dir 2", Lancamento = new DateOnly(2001, 1, 1) });

        var filmes = _filmeRepo.ListarFilmes();

        Assert.Equal(2, filmes.Count);
        Assert.Contains(filmes, f => f.Titulo == "Filme 1");
    }

    [Fact]
    public void BuscarPorId_FilmeExistente_DeveRetornarFilme()
    {
        _filmeRepo.Adicionar(new Filme { Titulo = "Filme Existente", Diretor = "Dir Ex", Lancamento = new DateOnly(2005, 1, 1) });

        var filmeEncontrado = _filmeRepo.BuscarPorId(1);

        Assert.NotNull(filmeEncontrado);
        Assert.Equal("Filme Existente", filmeEncontrado.Titulo);
    }

    [Fact]
    public void Atualizar_FilmeExistente_DeveAlterarDados()
    {
        _filmeRepo.Adicionar(new Filme { Titulo = "Filme Antigo", Diretor = "Dir A", Lancamento = new DateOnly(1990, 1, 1) });
        var filmeParaAtualizar = _filmeRepo.BuscarPorId(1);

        filmeParaAtualizar.Titulo = "Filme Novo";
        _filmeRepo.Atualizar(filmeParaAtualizar);

        var filmeAtualizado = _filmeRepo.BuscarPorId(1);
        Assert.NotNull(filmeAtualizado);
        Assert.Equal("Filme Novo", filmeAtualizado.Titulo);
    }

    [Fact]
    public void Deletar_FilmeExistente_DeveRemoverDoBanco()
    {
        _filmeRepo.Adicionar(new Filme { Titulo = "Filme para Deletar", Diretor = "Dir D", Lancamento = new DateOnly(2020, 1, 1) });
        var idParaDeletar = 1;

        _filmeRepo.Deletar(idParaDeletar);

        var filmeDeletado = _filmeRepo.BuscarPorId(idParaDeletar);
        Assert.Null(filmeDeletado);
    }

    [Fact]
    public void BuscarPorId_IdInexistente_DeveRetornarNull()
    {
        var idInexistente = 999;

        var filmeEncontrado = _filmeRepo.BuscarPorId(idInexistente);

        Assert.Null(filmeEncontrado);
    }
}