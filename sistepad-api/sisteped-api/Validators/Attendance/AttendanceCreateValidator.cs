using FluentValidation;
using SistepedApi.DTOs.Request;

namespace SistepedApi.Validators.Attendance
{
    public class AttendanceCreateValidator : AbstractValidator<AttendanceCreateDTO>
    {
        public AttendanceCreateValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("O ID do aluno é obrigatório.");

            RuleFor(x => x.ClassId)
                .GreaterThan(0).WithMessage("O ID da matéria é obrigatório.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("A data é obrigatória.");
        }
    }
}
