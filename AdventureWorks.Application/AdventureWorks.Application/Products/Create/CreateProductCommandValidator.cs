using FluentValidation;

namespace AdventureWorks.Application.Products.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ProductNumber).NotEmpty().MaximumLength(25);
        RuleFor(x => x.StandardCost).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ListPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.SafetyStockLevel).GreaterThanOrEqualTo((short)0);
        RuleFor(x => x.ReorderPoint).GreaterThanOrEqualTo((short)0);
        RuleFor(x => x.SellEndDate).GreaterThan(x => x.SellStartDate).When(x => x.SellEndDate.HasValue);
    }
}
