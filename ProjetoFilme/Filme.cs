namespace ProjetoFilme;

public class Filme
{
    public int Id { get; set; }
    public required string Titulo { get; set; }
    public required string Diretor { get; set; }
    public required DateOnly Lancamento { get; set; }
    public string? Genero { get; set; }
    public double Avaliacao { get; set; }
    public string? Sinopse { get; set; }
}
