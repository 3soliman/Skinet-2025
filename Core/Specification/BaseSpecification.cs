using System.Linq.Expressions;

namespace Core.Specification;

public class BaseSpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; protected set; }

    public BaseSpecification()
    {
    }

    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }
}


