using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    protected ActionResult<object> CreatePagedResponse<T>(IEnumerable<T> data, int pageIndex, int pageSize, int totalItems)
    {
        var response = new
        {
            pageIndex,
            pageSize,
            totalItems,
            data
        };
        return Ok(response);
    }
}
