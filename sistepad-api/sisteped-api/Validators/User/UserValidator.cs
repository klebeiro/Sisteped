using FluentValidation;
using SistepedApi.DTOs.Request;
using SistepedApi.Models.Enums;

namespace SistepedApi.Validators.User
{
    public class UserValidator : AbstractValidator<UserCreateDTO>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório.")
                .EmailAddress().WithMessage("Email não está no formato válido.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(100).WithMessage("Nome não pode conter mais de 100 caracteres.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(8).WithMessage("Senha deve possuir pelo menos 8 caracteres")
                .Equal(x => x.PasswordConfirmation).WithMessage("Senha e confirmação de senha devem ser iguais");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Tipo de usuário inválido. Use: 1 (Coordenador), 2 (Professor) ou 3 (Responsável).");
        }
    }

    public class UserCredentialDTOValidator : AbstractValidator<UserCredentialDTO>
    {
        public UserCredentialDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório.")
                .EmailAddress().WithMessage("Email não está no formato válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(8).WithMessage("Senha deve possuir pelo menos 8 caracteres");
        }
    }
}