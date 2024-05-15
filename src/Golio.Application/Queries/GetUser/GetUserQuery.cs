using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Application.ViewModels;
using MediatR;

namespace Golio.Application.Queries.GetUser
{
    public class GetUserQuery : IRequest<UserViewModel>
    {
        public GetUserQuery(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}