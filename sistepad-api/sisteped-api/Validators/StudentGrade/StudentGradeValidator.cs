using FluentValidation;
using SistepedApi.DTOs.Request;

namespace SistepedApi.Validators.StudentGrade
{
    public class StudentGradeValidator : AbstractValidator<StudentGradeDTO>
    {
        public StudentGradeValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("O ID do aluno é obrigatório.");

            RuleFor(x => x.GradeId)
                .GreaterThan(0).WithMessage("O ID da turma é obrigatório.");
        }
    }
}
