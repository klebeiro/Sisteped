using FluentValidation;
using SistepedApi.DTOs.Request;

namespace SistepedApi.Validators.Class
{
    public class ClassUpdateValidator : AbstractValidator<ClassUpdateDTO>
    {
        public ClassUpdateValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("O código da matéria é obrigatório.")
                .MaximumLength(50).WithMessage("O código da matéria deve ter no máximo 50 caracteres.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome da matéria é obrigatório.")
                .MaximumLength(150).WithMessage("O nome da matéria deve ter no máximo 150 caracteres.");

            RuleFor(x => x.WorkloadHours)
                .GreaterThan(0).WithMessage("A carga horária deve ser maior que zero.");
        }
    }
}
