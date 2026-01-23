using FluentValidation;
using SistepedApi.DTOs.Request;

namespace SistepedApi.Validators.Activity
{
    public class ActivityCreateValidator : AbstractValidator<ActivityCreateDTO>
    {
        public ActivityCreateValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("O título da atividade é obrigatório.")
                .MaximumLength(200).WithMessage("O título da atividade deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("A descrição deve ter no máximo 1000 caracteres.");

            RuleFor(x => x.ClassId)
                .GreaterThan(0).WithMessage("A matéria é obrigatória.");

            RuleFor(x => x.ApplicationDate)
                .NotEmpty().WithMessage("A data de aplicação é obrigatória.");

            RuleFor(x => x.MaxScore)
                .GreaterThan(0).WithMessage("A nota máxima deve ser maior que zero.")
                .LessThanOrEqualTo(1000).WithMessage("A nota máxima deve ser no máximo 1000.");
        }
    }
}
