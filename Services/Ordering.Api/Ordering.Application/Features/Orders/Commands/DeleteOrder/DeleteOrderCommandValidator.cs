using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandValidator: AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(d => d.Id).NotEqual(0)
                .WithMessage("No se puede borrar registro 0")
                .NotNull()
                .WithMessage("No se puede eliminar numero null");

        }
    }
}
