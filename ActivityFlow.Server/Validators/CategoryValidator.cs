using FluentValidation;
using ActivityFlow.Server.Models;

namespace ActivityFlow.Server.Validators;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("El nombre es requerido y no debe exceder 50 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(100)
            .When(x => x.Description != null)
            .WithMessage("La descripción no debe exceder 100 caracteres");
    }
} 