using FluentValidation;
using SistepedApi.DTOs.Request;

namespace SistepedApi.Validators.Grade
{
    public class GradeCreateValidator : AbstractValidator<GradeCreateDTO>
    {
        public GradeCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome da série é obrigatório.")
                .MaximumLength(100).WithMessage("O nome da série deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Level)
                .GreaterThan(0).WithMessage("O nível deve ser maior que zero.");

            RuleFor(x => x.Shift)
                .InclusiveBetween(1, 3).WithMessage("O turno deve ser entre 1 (Manhã), 2 (Tarde) ou 3 (Noite).");
        }
    }
}
