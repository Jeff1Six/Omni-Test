using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartValidator : AbstractValidator<CreateCartCommand>
{
    public CreateCartValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Date)
            .NotEmpty();

        RuleFor(x => x.Products)
            .NotNull()
            .Must(x => x.Count > 0)
            .WithMessage("Cart must have at least one product.");

        RuleForEach(x => x.Products).ChildRules(p =>
        {
            p.RuleFor(x => x.ProductId).NotEmpty();
            p.RuleFor(x => x.Quantity).GreaterThan(0);
        });
    }
}
