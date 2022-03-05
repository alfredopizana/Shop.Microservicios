using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IAsyncRepository<Order> repository;
        private readonly ILogger<DeleteOrderCommandHandler> logger;

        public DeleteOrderCommandHandler(IAsyncRepository<Order> repository, ILogger<DeleteOrderCommandHandler> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await repository.GetByIdAsync(request.Id);

            if(orderToUpdate == null)
            {
                //logguear error
                logger.LogError("La orden no existe");

            }
            await repository.DeleteAsync(orderToUpdate);

            return Unit.Value;
        }
    }
}
