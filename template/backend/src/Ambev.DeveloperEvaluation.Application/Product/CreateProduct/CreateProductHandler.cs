using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;

    public CreateProductHandler(DefaultContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request);

        if (product.Id == Guid.Empty)
            product.Id = Guid.NewGuid();

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CreateProductResult>(product);
    }
}
