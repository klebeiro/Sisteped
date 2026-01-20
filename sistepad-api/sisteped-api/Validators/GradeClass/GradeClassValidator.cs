using FluentValidation;
using SistepedApi.DTOs.Request;

namespace SistepedApi.Validators.GradeClass
{
    public class GradeClassValidator : AbstractValidator<GradeClassDTO>
    {
        public GradeClassValidator()
        {
            RuleFor(x => x.GradeId)
                .GreaterThan(0).WithMessage("O ID da série é obrigatório.");

            RuleFor(x => x.ClassId)
                .GreaterThan(0).WithMessage("O ID da matéria é obrigatório.");
        }
    }
}
