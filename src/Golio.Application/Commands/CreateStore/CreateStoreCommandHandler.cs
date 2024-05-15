using System;
using System.Threading;
using System.Threading.Tasks;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using MediatR;

namespace Golio.Application.Commands.CreateProduct.CreateUser
{
    public class CreateStoreCommandHandler : IRequestHandler<CreateStoreCommand, int>
    {
        private readonly IStoreRepository _storeRepository;

        public CreateStoreCommandHandler(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }
        public async Task<int> Handle(CreateStoreCommand request, CancellationToken cancellationToken)
        {
            var price = new Store
            {
                Name = request.Name,
                Address = request.Address,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                CreatedAt = DateTime.UtcNow
            };

            await _storeRepository.AddStoreAsync(price);


            return price.Id;
        }
    }
}