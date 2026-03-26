using CutoverManager.Application.DTOs;
using FluentValidation;

namespace CutoverManager.Application.Validators;

public class ExecutorValidator : AbstractValidator<ExecutorDTO>
{
    public ExecutorValidator()
    {
        RuleFor(e => e.Nome).NotEmpty().MaximumLength(250);
    }
}