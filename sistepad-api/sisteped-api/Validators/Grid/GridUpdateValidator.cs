using FluentValidation;
using SistepedApi.DTOs.Request;

namespace SistepedApi.Validators.Grid
{
    public class GridUpdateValidator : AbstractValidator<GridUpdateDTO>
    {
        public GridUpdateValidator()
        {
            RuleFor(x => x.Year)
                .GreaterThan(2000).WithMessage("O ano deve ser maior que 2000.")
                .LessThanOrEqualTo(DateTime.Now.Year + 1).WithMessage("O ano não pode ser maior que o próximo ano.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");
        }
    }
}
