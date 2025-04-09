using FluentValidation;
using ActivityFlow.Server.Models;
using ActivityFlow.Shared.Enums;

namespace ActivityFlow.Server.Validators;

public class StatusValidator : AbstractValidator<Status>
{
    public StatusValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre del estado es requerido");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("La descripción no puede exceder los 500 caracteres");

        RuleFor(x => x.Color)
            .MaximumLength(50)
            .WithMessage("El color no puede exceder los 50 caracteres");

        RuleFor<int>(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El orden debe ser un número positivo");
    }
} 