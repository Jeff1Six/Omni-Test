using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.ListCarts;

public sealed class ListCartsValidator : AbstractValidator<ListCartsQuery>
{
    public ListCartsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("_page must be greater than 0.");

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .WithMessage("_size must be greater than 0.");
    }
}
