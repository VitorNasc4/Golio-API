using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Golio.Application.Commands.UserCommands.CreateUser;
using FluentValidation;
using Golio.Application.Commands.LoginUser;

namespace Golio.Application.Validators
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(u => u.Email)
                .EmailAddress()
                .WithMessage("E-mail não válido");
        }

        public static bool ValidPassword(string password)
        {
            return password.Length >= 8;
        }
    }
}