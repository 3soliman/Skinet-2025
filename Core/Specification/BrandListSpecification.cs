using System.Linq.Expressions;
using Core.Entities;
using Core.Interfaces;

namespace Core.Specification;

public class BrandListSpecification : BaseSpecification<Product, string>
{
    public BrandListSpecification()
        : base(p => p.Brand)
    {
        Criteria = p => !string.IsNullOrEmpty(p.Brand);
    }
}


