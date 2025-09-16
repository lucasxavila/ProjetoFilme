using System.Globalization;

namespace ProjetoFilme;

public class Program
{
    public static void Main(string[] args)
    {
        var databasePath = "filmes.db";
        var filmeRepositorio = new FilmeRepositorio(databasePath);

        bool continuar = true;
        while (continuar)
        {
            Console.WriteLine("--- Gerenciador de Filmes ---");
            Console.WriteLine("1. Adicionar novo filme");
            Console.WriteLine("2. Listar todos os filmes");
            Console.WriteLine("3. Buscar filme por ID");
            Console.WriteLine("4. Atualizar filme");
            Console.WriteLine("5. Deletar filme");
            Console.WriteLine("0. Sair");
            Console.Write("Escolha uma opção: ");

            var opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    AdicionarNovoFilme(filmeRepositorio);
                    break;
                case "2":
                    ListarTodosFilmes(filmeRepositorio);
                    break;
                case "3":
                    BuscarFilmePorId(filmeRepositorio);
                    break;
                case "4":
                    AtualizarFilme(filmeRepositorio);
                    break;
                case "5":
                    DeletarFilme(filmeRepositorio);
                    break;
                case "0":
                    continuar = false;
                    break;
            }

            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    public static void AdicionarNovoFilme(FilmeRepositorio repo)
    {
        Console.WriteLine("\n--- Adicionar Novo Filme ---");

        Console.Write("Título: ");
        var titulo = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(titulo))
        {
            Console.WriteLine("O título é obrigatório.");
            return;
        }

        Console.Write("Diretor: ");
        var diretor = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(diretor))
        {
            Console.WriteLine("O diretor é obrigatório.");
            return;
        }

        Console.Write("Data de Lançamento (dd/MM/yyyy): ");
        var lancamentoString = Console.ReadLine();

        DateOnly lancamento;

        if (string.IsNullOrWhiteSpace(lancamentoString) ||
            !DateOnly.TryParseExact(lancamentoString, "dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"), DateTimeStyles.None, out lancamento) ||
            lancamento > DateOnly.FromDateTime(DateTime.Now))
        {
            Console.WriteLine("Data de lançamento é obrigatória, deve estar no formato ('dd/MM/yyyy') e não pode estar no futuro. Tente novamente!");
            return;
        }

        Console.Write("Gênero: ");
        var genero = Console.ReadLine();

        Console.Write("Avaliação ('0-5'): ");
        var entrada = Console.ReadLine();

        bool conversao = double.TryParse(entrada, out var avaliacao);
        if (!conversao || avaliacao > 5)
        {
            Console.WriteLine("Avaliação inválida! Digite apenas números e entre 0 e 5 (pode ser decimal).");
            return;
        }

        Console.Write("Sinopse: ");
        var sinopse = Console.ReadLine();

        var novoFilme = new Filme
        {
            Titulo = titulo,
            Diretor = diretor,
            Lancamento = lancamento,
            Genero = genero,
            Avaliacao = avaliacao,
            Sinopse = sinopse,
        };

        repo.Adicionar(novoFilme);
        Console.WriteLine("\nFilme adicionado com sucesso!");
    }

    public static void ListarTodosFilmes(FilmeRepositorio repo)
    {
        Console.WriteLine("\n--- Lista de Filmes ---");

        var filmes = repo.ListarFilmes();

        if (filmes.Count == 0)
        {
            Console.WriteLine("Nenhum filme encontrado. Cadastre um novo filme para começar.");
        }
        else
        {
            foreach (var filme in filmes)
            {
                Console.WriteLine($"ID: {filme.Id}");
                Console.WriteLine($"Título: {filme.Titulo}");
                Console.WriteLine($"Diretor: {filme.Diretor}");
                Console.WriteLine($"Lançamento: {filme.Lancamento.ToString("dd/MM/yyy")}");
                Console.WriteLine($"Gênero: {filme.Genero ?? "N/A"}");
                Console.WriteLine($"Avaliação: {filme.Avaliacao}");
                Console.WriteLine($"Sinopse: {filme.Sinopse ?? "N/A"}");
                Console.WriteLine("-----------------------------------");
            }
        }
    }

    public static void BuscarFilmePorId(FilmeRepositorio repo)
    {
        Console.WriteLine("\n--- Buscar filme por ID ---");
        Console.Write("Digite o ID do filme desejado: ");

        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var filme = repo.BuscarPorId(id);

            if (filme != null)
            {
                Console.WriteLine("\nFilme encontrado:");
                Console.WriteLine($"ID: {filme.Id}");
                Console.WriteLine($"Título: {filme.Titulo}");
                Console.WriteLine($"Diretor: {filme.Diretor}");
                Console.WriteLine($"Lançamento: {filme.Lancamento.ToString("dd/MM/yyy")}");
                Console.WriteLine($"Gênero: {filme.Genero ?? "N/A"}");
                Console.WriteLine($"Avaliação: {filme.Avaliacao}");
                Console.WriteLine($"Sinopse: {filme.Sinopse ?? "N/A"}");
            }
            else
            {
                Console.WriteLine("Filme não encontrado");
            }
        }
        else
        {
            Console.WriteLine("ID inválido. Por favor, digite um número.");
        }
    }

    public static void AtualizarFilme(FilmeRepositorio repo)
    {
        Console.WriteLine("\n--- Atualizar Filme ---");
        Console.Write("Digite o ID do filme a ser atualizado: ");

        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var filmeExistente = repo.BuscarPorId(id);

            if (filmeExistente == null)
            {
                Console.WriteLine("Filme não encontrado.");
                return;
            }

            Console.WriteLine("\nFilme encontrado! \nInsira os novos dados (deixe em branco para manter o valor atual)");

            Console.Write($"Novo Título (atual: {filmeExistente.Titulo}): ");
            var novoTitulo = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoTitulo))
            {
                filmeExistente.Titulo = novoTitulo;
            }

            Console.Write($"Novo Diretor (atual: {filmeExistente.Diretor}): ");
            var novoDiretor = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoDiretor))
            {
                filmeExistente.Diretor = novoDiretor;
            }

            Console.Write($"Novo Lançamento 'dd/MM/yyyy' (atual: {filmeExistente.Lancamento.ToString("dd/MM/yyyy")}): ");
            var novoLancamentoString = Console.ReadLine();
            DateOnly novoLancamento;

            if (!string.IsNullOrWhiteSpace(novoLancamentoString))
            {
                if (DateOnly.TryParseExact(novoLancamentoString, "dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"), DateTimeStyles.None, out novoLancamento))
                {
                    if (novoLancamento <= DateOnly.FromDateTime(DateTime.Now))
                    {
                        filmeExistente.Lancamento = novoLancamento;
                    }
                    else
                    {
                        Console.WriteLine("A data de lançamento não pode estar no futuro. A data não foi atualizada!");
                    }
                }
                else
                {
                    Console.WriteLine("Formato de data inválido. A data não foi atualizada!");
                }
            }

            Console.Write($"Novo Gênero (atual: {filmeExistente.Genero}): ");
            var novoGenero = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoGenero))
            {
                filmeExistente.Genero = novoGenero;
            }

            Console.Write($"Nova Avaliação '0-5' (atual: {filmeExistente.Avaliacao}): ");
            var novaAvaliacaoString = Console.ReadLine();
            double novaAvaliacao;

            if (!string.IsNullOrWhiteSpace(novaAvaliacaoString))
            {
                {
                    if (double.TryParse(novaAvaliacaoString, out novaAvaliacao))
                    {
                        if (novaAvaliacao >= 0 && novaAvaliacao <= 5)
                        {
                            filmeExistente.Avaliacao = novaAvaliacao;
                        }
                        else
                        {
                            Console.WriteLine("Valor inválido. A avaliação deve estar entre 0 e 5.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Formato de avaliação inválido. Por favor, digite um número.");
                    }
                }
            }

            Console.Write($"Nova Sinopse: (atual: {filmeExistente.Sinopse}): ");
            var novaSinopse = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(novaSinopse))
            {
                filmeExistente.Sinopse = novaSinopse;
            }
            repo.Atualizar(filmeExistente);
            Console.WriteLine("\nFilme atualizado com sucesso!");
        }
        else
        {
            Console.WriteLine("ID inválido. Por favor, digite um número.");
        }
    }

    public static void DeletarFilme(FilmeRepositorio repo)
    {
        Console.WriteLine("\n--- Deletar Filme ---");
        Console.Write("Digite o ID do filme a ser deletado: ");

        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var filmeExistente = repo.BuscarPorId(id);

            if (filmeExistente == null)
            {
                Console.WriteLine("Filme não encontrado.");
                return;
            }

            Console.WriteLine($"\nVocê tem certeza que deseja deletar o filme '{filmeExistente.Titulo}'?");
            Console.Write("Digite 'sim' para confirmar ou qualquer outra coisa para cancelar: ");

            var confirmacao = Console.ReadLine();

            if (confirmacao?.ToLower() == "sim")
            {
                repo.Deletar(id);
                Console.WriteLine("Filme deletado com sucesso!");
            }
            else
            {
                Console.WriteLine("Operação cancelada!");
            }
        }
    }
}