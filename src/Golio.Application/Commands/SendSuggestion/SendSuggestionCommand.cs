using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Core.Entities;
using MediatR;

namespace Golio.Application.Commands.CreateProduct.SendSuggestion
{
    public class SendSuggestionCommand : IRequest<Unit>
    {
        public int PriceId { get; set; }
        public double NewPrice { get; set; }
        public bool IsValid { get; set; }
    }
}