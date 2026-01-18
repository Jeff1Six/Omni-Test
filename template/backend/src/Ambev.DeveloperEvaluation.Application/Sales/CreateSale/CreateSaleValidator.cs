using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public sealed class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.CustomerName).NotEmpty();

        RuleFor(x => x.BranchId).NotEmpty();
        RuleFor(x => x.BranchName).NotEmpty();

        RuleFor(x => x.Items)
            .NotNull()
            .NotEmpty();

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId).NotEmpty();
            item.RuleFor(i => i.ProductTitle).NotEmpty();

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0)
                .LessThanOrEqualTo(20);

            item.RuleFor(i => i.UnitPrice)
                .GreaterThan(0);
        });
    }
}
