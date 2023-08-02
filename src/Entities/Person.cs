namespace PersonApi.Entities;

public record Person(
    Guid Id,
    string Apelido,
    string Nome,
    string Nascimento,
    IEnumerable<string>? Stack
);
