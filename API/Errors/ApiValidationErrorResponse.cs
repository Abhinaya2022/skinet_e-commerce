using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors;

public class ApiValidationErrorResponse : ApiErrorResponse
{
    public ApiValidationErrorResponse() : base(StatusCodes.Status400BadRequest)
    {
    }

    public IEnumerable<string> Errors { get; set; }
}
