using FluentValidation;
using SistepedApi.DTOs.Request;

namespace SistepedApi.Validators.StudentActivity
{
    public class StudentActivityCreateValidator : AbstractValidator<StudentActivityCreateDTO>
    {
        public StudentActivityCreateValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("O aluno é obrigatório.");

            RuleFor(x => x.ActivityId)
                .GreaterThan(0).WithMessage("A atividade é obrigatória.");

            RuleFor(x => x.Score)
                .GreaterThanOrEqualTo(0).When(x => x.Score.HasValue)
                .WithMessage("A nota não pode ser negativa.");

            RuleFor(x => x.Remarks)
                .MaximumLength(500).WithMessage("As observações devem ter no máximo 500 caracteres.");
        }
    }
}
