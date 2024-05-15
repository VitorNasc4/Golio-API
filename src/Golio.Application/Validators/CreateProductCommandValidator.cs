using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Golio.Application.Commands.UserCommands.CreateUser;
using FluentValidation;
using Golio.Application.Commands.LoginUser;
using Golio.Application.Commands.CreateProduct.CreateUser;
using Golio.Core.Entities;
using Golio.Application.InputModels;

namespace Golio.Application.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(product => product.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("O nome do produto é obrigatório");

            RuleFor(product => product.Brand)
                .NotNull()
                .NotEmpty()
                .WithMessage("A marca do produto é obrigatório");

            RuleFor(product => product.Volume)
                .NotNull()
                .NotEmpty()
                .WithMessage("O volume do produto é obrigatório");

            RuleForEach(product => product.Prices)
                .SetValidator(new PriceValidator());
        }

        public static bool ValidPassword(string password)
        {
            return password.Length >= 8;
        }
    }

    public class PriceValidator : AbstractValidator<CreatePriceInputModel>
    {
        public PriceValidator()
        {
            RuleFor(price => price.Value)
            .GreaterThan(0)
            .NotNull()
            .NotEmpty()
            .WithMessage("O valor preço é obrigatória");

            RuleFor(price => price.Store)
                .SetValidator(new StoreValidator());
        }
    }

    public class StoreValidator : AbstractValidator<CreateStoreInputModel>
    {
        public StoreValidator()
        {
            RuleFor(store => store.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("O nome da loja é obrigatória");

            RuleFor(store => store.Address)
            .NotNull()
            .NotEmpty()
            .WithMessage("O endereço da loja é obrigatória");

            RuleFor(store => store.City)
            .NotNull()
            .NotEmpty()
            .WithMessage("A cidade da loja é obrigatória");

            RuleFor(store => store.State)
            .NotNull()
            .NotEmpty()
            .WithMessage("O estado da loja é obrigatória");

            RuleFor(store => store.ZipCode)
            .NotNull()
            .NotEmpty()
            .WithMessage("O CEP da loja é obrigatória");
        }
    }
}