namespace Core.Specification;

public class ProductSpecParams
{
    public string[]? Brands { get; set; }
    public string[]? Types { get; set; }
    public string? Search { get; set; }
    public string? Sort { get; set; }

    private const int MaxPageSize = 50;
    private int _pageSize = 6;

    public int PageIndex { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : (value <= 0 ? 10 : value);
    }
}


