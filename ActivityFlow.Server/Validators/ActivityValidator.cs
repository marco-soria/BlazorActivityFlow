using FluentValidation;
using ActivityFlow.Server.Models;

namespace ActivityFlow.Server.Validators;

public class ActivityValidator : AbstractValidator<Activity>
{
    public ActivityValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("El título es requerido y no debe exceder 200 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => x.Description != null)
            .WithMessage("La descripción no debe exceder 1000 caracteres");

        RuleFor(x => x.DueDate)
            .Must(x => x == null || x > DateTime.UtcNow)
            .WithMessage("La fecha de vencimiento debe ser futura");

        RuleFor(x => x.Priority)
            .InclusiveBetween(0, 5)
            .WithMessage("La prioridad debe estar entre 0 y 5");

        RuleFor(x => x.StatusId)
            .NotEmpty()
            .WithMessage("El estado es requerido");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("El usuario creador es requerido");
    }
} 