using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    private readonly StoreContext _context;

    public BuggyController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet("testauth")]
    [Authorize]
    public ActionResult<string> GetSecretText()
    {
        return "secret stuff";
    }

    [HttpGet("notfound")]
    public ActionResult GetNotFoundRequest()
    {
        return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
    }

    [HttpGet("servererror")]
    public ActionResult GetServerError()
    {
        string servererror = null;
        servererror.ToString();
        return Ok(new ApiErrorResponse(StatusCodes.Status500InternalServerError));
    }

    [HttpGet("badrequest")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
    }

    [HttpGet("badrequest/{id}")]
    public ActionResult GetBadRequest(int id)
    {
        return BadRequest(StatusCodes.Status400BadRequest);
    }
}
