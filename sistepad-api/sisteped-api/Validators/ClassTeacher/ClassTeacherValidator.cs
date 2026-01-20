using FluentValidation;
using SistepedApi.DTOs.Request;

namespace SistepedApi.Validators.ClassTeacher
{
    public class ClassTeacherValidator : AbstractValidator<ClassTeacherDTO>
    {
        public ClassTeacherValidator()
        {
            RuleFor(x => x.ClassId)
                .GreaterThan(0).WithMessage("O ID da matéria é obrigatório.");

            RuleFor(x => x.TeacherId)
                .GreaterThan(0).WithMessage("O ID do professor é obrigatório.");
        }
    }
}
