using System;
using Entities.ErrorModel;
using Entities.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Products.Presentation.APIS;

public class ApiControllerBase : ControllerBase
{
    protected IActionResult ProcessError(ApiBaseResponse baseResponse)
    {
        return baseResponse switch
        {
            ApiNotFoundResponse response => NotFound(new ErrorDetails
            {
                Message = response.Message,
                StatusCode = StatusCodes.Status404NotFound
            }),
            ApiBadRequestResponse response => BadRequest(new ErrorDetails
            {
                Message = response.Message,
                StatusCode = StatusCodes.Status400BadRequest
            }),
            _ => throw new Exception("Unable to process error.")
        };
    }
}