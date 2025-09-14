using System.Linq.Expressions;
using Core.Entities;
using Core.Interfaces;

namespace Core.Specification;

public class TypeListSpecification : BaseSpecification<Product, string>
{
    public TypeListSpecification()
        : base(p => p.Type)
    {
        Criteria = p => !string.IsNullOrEmpty(p.Type);
    }
}


