using Microsoft.AspNetCore.Mvc;
using API.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        // GET: api/buggy/notfound
        [HttpGet("notfound")]
        public ActionResult GetNotFound()
        {
            return NotFound(new { StatusCode = 404, Message = "Resource not found" });
        }

        // GET: api/buggy/badrequest
        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new { StatusCode = 400, Message = "This is a bad request" });
        }

        // POST: api/buggy/validationerror
        [HttpPost("validationerror")]
        public ActionResult GetValidationError(CreateProductDto productDto)
        {

            return Ok(productDto);
        }

        // GET: api/buggy/unauthorised
        [HttpGet("unauthorised")]
        public ActionResult GetUnauthorised()
        {
            return Unauthorized(new { StatusCode = 401, Message = "You are not authorized" });
        }

        // GET: api/buggy/servererror
        [HttpGet("servererror")]
        public ActionResult GetInternalServerError()
        {
            throw new Exception("This is a test server error"); 
        }

        // GET: api/buggy/testcors
        [HttpGet("testcors")]
        public ActionResult GetCorsTest()
        {
            return Ok("CORS is enabled");
        }
    }

    // DTO تجريبي علشان اختبار الفاليديشن
    public class TestDto
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }
    }
}
