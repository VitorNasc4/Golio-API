using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Core.Entities;
using MediatR;

namespace Golio.Application.Commands.CreateProduct.SendSuggestion
{
    public class RemoveSuggestionCommand : IRequest<Unit>
    {
        public int SuggestionId { get; set; }
    }
}