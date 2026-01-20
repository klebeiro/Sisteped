using FluentValidation;
using SistepedApi.DTOs.Request;

namespace SistepedApi.Validators.Grid
{
    public class GridGradeValidator : AbstractValidator<GridGradeDTO>
    {
        public GridGradeValidator()
        {
            RuleFor(x => x.GridId)
                .GreaterThan(0).WithMessage("O ID da grade é obrigatório.");

            RuleFor(x => x.GradeId)
                .GreaterThan(0).WithMessage("O ID da série é obrigatório.");
        }
    }
}
