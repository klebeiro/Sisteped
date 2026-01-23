using FluentValidation;
using SistepedApi.DTOs.Request;

namespace SistepedApi.Validators.GridClass
{
    public class GridClassValidator : AbstractValidator<GridClassDTO>
    {
        public GridClassValidator()
        {
            RuleFor(x => x.GridId)
                .GreaterThan(0).WithMessage("O ID da grade curricular é obrigatório.");

            RuleFor(x => x.ClassId)
                .GreaterThan(0).WithMessage("O ID da matéria é obrigatório.");
        }
    }
}
