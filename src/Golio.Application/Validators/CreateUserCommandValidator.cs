using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Golio.Application.Commands.UserCommands.CreateUser;
using FluentValidation;

namespace Golio.Application.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(u => u.Email)
                .EmailAddress()
                .WithMessage("E-mail não válido");

            RuleFor(u => u.Password)
                .Must(ValidPassword)
                .WithMessage("A senha deve ter pelo menos 8 caracteres");

            RuleFor(u => u.FullName)
                .NotNull()
                .NotEmpty()
                .WithMessage("O nome é obrigatório");
        }

        public static bool ValidPassword(string password)
        {
            return password.Length >= 8;
        }
    }
}