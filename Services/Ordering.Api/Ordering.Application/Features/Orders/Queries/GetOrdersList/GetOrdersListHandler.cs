using AutoMapper;
using MediatR;
using Ordering.Application.Contracts;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListHandler : IRequestHandler<GetOrdersListQuery, List<OrdersViewModel>>
    {
        private readonly IAsyncRepository<Order> repository;
        private readonly IMapper mapper;

        public GetOrdersListHandler(IAsyncRepository<Order> repository, IMapper mapper)
        => (this.repository, this.mapper) = (repository, mapper);
        public async Task<List<OrdersViewModel>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var ordersList = await repository.GetAsync(o => o.UserName == request.UserName);
            return mapper.Map<List<OrdersViewModel>>(ordersList);
        }
    }
}
