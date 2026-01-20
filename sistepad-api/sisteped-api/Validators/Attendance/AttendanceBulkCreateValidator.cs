using FluentValidation;
using SistepedApi.DTOs.Request;

namespace SistepedApi.Validators.Attendance
{
    public class AttendanceBulkCreateValidator : AbstractValidator<AttendanceBulkCreateDTO>
    {
        public AttendanceBulkCreateValidator()
        {
            RuleFor(x => x.GradeId)
                .GreaterThan(0).WithMessage("O ID da série é obrigatório.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("A data é obrigatória.");

            RuleFor(x => x.Students)
                .NotEmpty().WithMessage("A lista de alunos é obrigatória.")
                .Must(students => students.All(s => s.StudentId > 0))
                .WithMessage("Todos os IDs de alunos devem ser válidos.");
        }
    }
}
