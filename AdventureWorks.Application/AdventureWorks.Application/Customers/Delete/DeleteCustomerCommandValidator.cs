using FluentValidation;

namespace AdventureWorks.Application.Customers.Delete;

internal sealed class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);
    }
}
