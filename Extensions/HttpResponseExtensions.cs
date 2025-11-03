using Microsoft.AspNetCore.Mvc;
using RealTimeTaskManager.Models;
using System.Net;

namespace RealTimeTaskManager.Extensions
{
    public static class HttpResponseExtensions
    {
        public static async Task<IActionResult> CreateSuccessResponse<T>(this HttpRequest req, T data, string message = "Success")
        {
            var apiResponse = new APIResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };

            return new OkObjectResult(apiResponse);
        }

        public static IActionResult CreateErrorResponse(this HttpRequest req, string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var apiResponse = new APIResponse<object>
            {
                Success = false,
                Message = errorMessage
            };

            return statusCode switch
            {
                HttpStatusCode.BadRequest => new BadRequestObjectResult(apiResponse),
                HttpStatusCode.NotFound => new NotFoundObjectResult(apiResponse),
                HttpStatusCode.InternalServerError => new ObjectResult(apiResponse) { StatusCode = (int)HttpStatusCode.InternalServerError },
                _ => new BadRequestObjectResult(apiResponse)
            };
        }

        public static IActionResult CreateBadRequestResponse(this HttpRequest req, string errorMessage)
        {
            return req.CreateErrorResponse(errorMessage, HttpStatusCode.BadRequest);
        }

        public static IActionResult CreateNotFoundResponse(this HttpRequest req, string errorMessage)
        {
            return req.CreateErrorResponse(errorMessage, HttpStatusCode.NotFound);
        }

        public static IActionResult CreateInternalServerErrorResponse(this HttpRequest req, string errorMessage)
        {
            return req.CreateErrorResponse(errorMessage, HttpStatusCode.InternalServerError);
        }
    }
}
