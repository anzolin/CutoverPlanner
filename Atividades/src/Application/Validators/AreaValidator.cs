using CutoverManager.Application.DTOs;
using FluentValidation;

namespace CutoverManager.Application.Validators;

public class AreaValidator : AbstractValidator<AreaDTO>
{
    public AreaValidator()
    {
        RuleFor(a => a.Nome).NotEmpty().MaximumLength(250);
    }
}