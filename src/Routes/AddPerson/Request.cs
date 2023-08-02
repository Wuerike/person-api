using PersonApi.Entities;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace PersonApi.Routes.AddPerson;

public record AddPersonRequest
{
    [Required]
    public string? Apelido { get; set; }

    [Required]
    public string? Nome { get; set; }

    [Required]
    public string? Nascimento { get; set; }

    public IEnumerable<string>? Stack { get; set; } = Enumerable.Empty<string>();


    public Person ToPerson()
    {
        return new Person(
            Id:new Guid(),
            Apelido: Apelido!,
            Nome: Nome!,
            Nascimento: Nascimento!,
            Stack: Stack
        );
    }

    public class Validator : AbstractValidator<AddPersonRequest>
    {
        public Validator()
        {
            RuleFor(r => r.Apelido)
                .NotEmpty()
                .MaximumLength(32);

            RuleFor(r => r.Nome)
                .NotEmpty()
                .MaximumLength(100)
                .Matches("^[a-zA-Z ]*$").WithMessage("'Nome' deve conter apenas letras maiúsculas, minúsculas e espaços.");

            RuleFor(r => r.Nascimento)
                .NotEmpty()
                .Must(p => DateOnly.TryParse(p, out _))
                .WithMessage("'Nascimento' deve ser uma data no formato AAAA-MM-DD.");

            When(r => !(r.Stack is null), () =>
            {
                RuleFor(r => r.Stack)
                    .Must(p => !p.Any(s => s.Length > 32))
                    .WithMessage($"'Stack' deve ser um array de strings de até 32 caracteres");
            });
        }
    }


}
