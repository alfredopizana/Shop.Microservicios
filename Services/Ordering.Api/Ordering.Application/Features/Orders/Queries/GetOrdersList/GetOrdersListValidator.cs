using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListValidator: AbstractValidator<GetOrdersListQuery>
    {
        public GetOrdersListValidator()
        {
            RuleFor(g => g.UserName).NotEqual("Admin");

        }
    }
}
