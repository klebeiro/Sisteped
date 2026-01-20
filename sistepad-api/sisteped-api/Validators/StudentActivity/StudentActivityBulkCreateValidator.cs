using FluentValidation;
using SistepedApi.DTOs.Request;

namespace SistepedApi.Validators.StudentActivity
{
    public class StudentActivityBulkCreateValidator : AbstractValidator<StudentActivityBulkCreateDTO>
    {
        public StudentActivityBulkCreateValidator()
        {
            RuleFor(x => x.ActivityId)
                .GreaterThan(0).WithMessage("A atividade é obrigatória.");

            RuleFor(x => x.Scores)
                .NotEmpty().WithMessage("A lista de notas não pode estar vazia.")
                .Must(scores => scores.All(s => s.StudentId > 0))
                .WithMessage("Todos os alunos devem ser válidos.");

            RuleForEach(x => x.Scores)
                .ChildRules(score =>
                {
                    score.RuleFor(s => s.StudentId)
                        .GreaterThan(0).WithMessage("O ID do aluno deve ser maior que zero.");
                    
                    score.RuleFor(s => s.Score)
                        .GreaterThanOrEqualTo(0).When(s => s.Score.HasValue)
                        .WithMessage("A nota não pode ser negativa.");
                    
                    score.RuleFor(s => s.Remarks)
                        .MaximumLength(500).WithMessage("As observações devem ter no máximo 500 caracteres.");
                });
        }
    }
}
