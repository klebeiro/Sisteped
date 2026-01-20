using FluentValidation;
using SistepedApi.DTOs.Request;

namespace SistepedApi.Validators.Student
{
    public class StudentUpdateValidator : AbstractValidator<StudentUpdateDTO>
    {
        public StudentUpdateValidator()
        {
            RuleFor(x => x.Enrollment)
                .NotEmpty().WithMessage("A matrícula é obrigatória.")
                .MaximumLength(50).WithMessage("A matrícula deve ter no máximo 50 caracteres.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(150).WithMessage("O nome deve ter no máximo 150 caracteres.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
                .LessThan(DateTime.Now).WithMessage("A data de nascimento deve ser anterior à data atual.");

            RuleFor(x => x.GuardianId)
                .GreaterThan(0).WithMessage("O ID do responsável é obrigatório.");
        }
    }
}
