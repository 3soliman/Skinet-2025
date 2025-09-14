using System.Linq.Expressions;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;

namespace Core.Specification;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(int id)
    {
        Criteria = p => p.Id == id;
    }

    public ProductSpecification(string? brand, string? type, string? sort)
    {
        Criteria = p =>
            (string.IsNullOrEmpty(brand) || p.Brand == brand) &&
            (string.IsNullOrEmpty(type) || p.Type == type);
            ApplySorting(sort);
    }

    public ProductSpecification(ProductSpecParams specParams)
    {
        var search = specParams.Search?.Trim().ToLower();
        Criteria = p =>
            (string.IsNullOrEmpty(search) || p.Name.ToLower().Contains(search)) &&
            (specParams.Brands == null || specParams.Brands.Length == 0 || specParams.Brands.Contains(p.Brand)) &&
            (specParams.Types == null || specParams.Types.Length == 0 || specParams.Types.Contains(p.Type));

        ApplySorting(specParams.Sort);
        if (specParams.PageIndex > 0 && specParams.PageSize > 0)
        {
            var skip = (specParams.PageIndex - 1) * specParams.PageSize;
            ApplyPaging(skip, specParams.PageSize);
        }
    }
     private void ApplySorting(string? sort)
    {
        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort.ToLower())
            {
                case "price":
                    AddOrderBy(p => p.Price);
                    break;
                case "pricedesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                case "name":
                    AddOrderBy(p => p.Name);
                    break;
                case "namedesc":
                    AddOrderByDescending(p => p.Name);
                    break;
                default:
                    AddOrderBy(p => p.Name); // الترتيب الافتراضي
                    break;
            }
        }
        else
        {
            AddOrderBy(p => p.Name); // الترتيب الافتراضي إذا لم يتم تحديد sort
        }
    }
}


