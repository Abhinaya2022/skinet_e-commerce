using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController:BaseApiController
{

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
