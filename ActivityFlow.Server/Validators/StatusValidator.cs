using FluentValidation;
using ActivityFlow.Server.Models;
using ActivityFlow.Shared.Enums;

namespace ActivityFlow.Server.Validators;

public class StatusValidator : AbstractValidator<Status>
{
    public StatusValidator()
    {
        RuleFor(x => x.Name)
            .IsInEnum()
            .WithMessage("El estado no es válido");

        RuleFor(x => x.Description)
            .MaximumLength(100)
            .When(x => x.Description != null)
            .WithMessage("La descripción no debe exceder 100 caracteres");

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El orden debe ser un número positivo");
    }
} 