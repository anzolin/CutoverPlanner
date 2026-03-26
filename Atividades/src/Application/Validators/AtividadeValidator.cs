using CutoverManager.Application.DTOs;
using FluentValidation;

namespace CutoverManager.Application.Validators;

public class AtividadeValidator : AbstractValidator<AtividadeDTO>
{
    public AtividadeValidator()
    {
        RuleFor(a => a.Titulo).NotEmpty().MaximumLength(250);
        RuleFor(a => a.Inicio).LessThanOrEqualTo(a => a.Termino)
            .WithMessage("Data de início não pode ser maior que a data de término.");
    }
}