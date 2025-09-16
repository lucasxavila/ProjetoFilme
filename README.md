# Projeto - Gerenciador de Filmes

Projeto realizado em **C#** para gerenciar um catálogo de filmes. O projeto implementa as funcionalidades básicas de um **CRUD** (Create, Read, Update, Delete) usando o **SQLite** como banco de dados.

## Propriedades

```
int ID: obrigatório;
string titulo: obrigatório;
string diretor: obrigatório;
DateOnly lançamento: obrigatório;
string genero: opcional;
double avaliacao: opcional;
string sinopse: opcional.
```

## Linguagem e Dependências

- Linguagem: C#;
- Banco de Dados: SQLite;
- Dependências:
```
dotnet add package Microsoft.Data.Sqlite
```

## Como Executar

1. Instale ou certifique-se de que o **.NET SDK** esteja instalado em sua máquina;
2. Instale as dependências na pasta raiz do projeto:
```
dotnet restore
```
3. Para executar:
```
dotnet run
```

## Funcionalidades

1. **Adicionar novo filme:** Permite cadastrar um novo filme. Os dados são solicitados um a um e possuem validação; 
2. **Listar todos os filmes:** Exibe todos os filmes cadastrados no banco de dados;
3. **Buscar filme por ID:** Solicita o ID de um filme e exibe seus detalhes se o registro for encontrado;
4. **Atualizar filme:** Permite modificar as informações de um filme existente;
5. **Deletar filme:** Solicita o ID de um filme, pede uma confirmação para evitar exclusões acidentais e remove o registro do banco de dados.

## Testes Unitários

Para garantir a qualidade do código, inclui testes unitários com xUnit. Eles seriam para validar o comportamento da classe FilmeRepositorio, cobrindo cenários de sucesso e de falha das operações de CRUD. 
Entretanto tive dificuldade para deixar os testes funcionando corretamente, sigo fazendo pesquisas para melhorar minhas habilidades nesse quesito.

### Para rodar os testes, execute o comando na pasta raiz do projeto:
```
dotnet test
```

## Contêinerização - Docker

Permite a execução da aplicação em qualquer ambiente de forma isolada e padronizada.

### Construindo a Imagem:
```
docker build -t filmes-app .
```

### Executando o Contêiner
```
docker run -it filmes-app
```

## Logs

Para registrar operações e fornecer feedback ao usuário, por ser uma aplicação menor, ela implementa um sistema de logging básico sem bibliotecas(Serilog ou NLog). 
Apenas utilizando a própria saída do console para registrar:
- Operações de Sucesso: Mensagens como "Filme adicionado com sucesso!" são exibidas para o usuário;
- Erros e Validações: Mensagens informativas como "Formato de data inválido." ou "O título é obrigatório." também são exibidas.
