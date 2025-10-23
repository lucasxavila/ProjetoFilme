namespace ProjetoFilme.Tests
{
    public class FilmeRepositorioTests : IDisposable
    {
        private readonly string _databasePath;
        private readonly FilmeRepositorio _repo;

        public FilmeRepositorioTests()
        {
            _databasePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.db");
            _repo = new FilmeRepositorio(_databasePath);
        }

        [Fact]
        public void Adicionar_DeveInserirFilmeNoBanco()
        {
            var filme = new Filme
            {
                Titulo = "Inception",
                Diretor = "Christopher Nolan",
                Lancamento = new DateOnly(2010, 7, 16),
                Genero = "Ficção Científica",
                Avaliacao = 4.8,
                Sinopse = "Um ladrão invade sonhos."
            };

            _repo.Adicionar(filme);
            var filmes = _repo.ListarFilmes();

            Assert.Single(filmes);
            Assert.Equal("Inception", filmes[0].Titulo);
            Assert.Equal("Christopher Nolan", filmes[0].Diretor);
        }

        [Fact]
        public void BuscarPorId_DeveRetornarFilmeCorreto()
        {
            var filme = new Filme
            {
                Titulo = "Matrix",
                Diretor = "Wachowski",
                Lancamento = new DateOnly(1999, 3, 31),
                Genero = "Ação",
                Avaliacao = 5.0
            };

            _repo.Adicionar(filme);
            var inserido = _repo.ListarFilmes()[0];

            var resultado = _repo.BuscarPorId(inserido.Id);

            Assert.NotNull(resultado);
            Assert.Equal("Matrix", resultado!.Titulo);
        }

        [Fact]
        public void Atualizar_DeveModificarFilmeExistente()
        {
            var filme = new Filme
            {
                Titulo = "Avatar",
                Diretor = "James Cameron",
                Lancamento = new DateOnly(2009, 12, 18),
                Genero = "Aventura",
                Avaliacao = 4.0
            };

            _repo.Adicionar(filme);
            var existente = _repo.ListarFilmes()[0];
            existente.Titulo = "Avatar (Remasterizado)";
            existente.Avaliacao = 4.5;

            _repo.Atualizar(existente);
            var atualizado = _repo.BuscarPorId(existente.Id);

            Assert.Equal("Avatar (Remasterizado)", atualizado!.Titulo);
            Assert.Equal(4.5, atualizado.Avaliacao);
        }

        [Fact]
        public void Deletar_DeveRemoverFilmeDoBanco()
        {
            var filme = new Filme
            {
                Titulo = "Titanic",
                Diretor = "James Cameron",
                Lancamento = new DateOnly(1997, 12, 19),
                Avaliacao = 5.0
            };

            _repo.Adicionar(filme);
            var existente = _repo.ListarFilmes()[0];

            _repo.Deletar(existente.Id);
            var filmes = _repo.ListarFilmes();

            Assert.Empty(filmes);
        }

        public void Dispose()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                if (File.Exists(_databasePath))
                    File.Delete(_databasePath);
            }
            catch (IOException)
            {
            }
        }
    }
}
