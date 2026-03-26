using CutoverManager.Application.DTOs;
using FluentValidation;

namespace CutoverManager.Application.Validators;

public class PlanoValidator : AbstractValidator<PlanoDTO>
{
    public PlanoValidator()
    {
        RuleFor(p => p.Nome).NotEmpty().MaximumLength(250);
        RuleFor(p => p.Inicio).LessThanOrEqualTo(p => p.Termino)
            .WithMessage("Data de início não pode ser maior que a data de término.");
    }
}