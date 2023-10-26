using Contracts.Interfaces.ControllerHelpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Implementations.ControllerHelperImplementations
{
    public class ErrorHandlingService : IErrorHandlingService
    {
        public IActionResult HandleException(Exception ex)
        {
            if (ex is HttpRequestException)
            {
                return HandleHttpRequestException((HttpRequestException)ex);
            }

            // Add more specific exception handling as needed

            // Generic fallback
            return HandleGenericException(ex);
        }

        private IActionResult HandleHttpRequestException(HttpRequestException ex)
        {
            // Handle specific HttpRequestException logic
            return new ObjectResult($"API request failed: {ex.Message}")
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }

        private IActionResult HandleGenericException(Exception ex)
        {
            // Handle generic exception logic
            return new ObjectResult($"An error occurred: {ex.Message}")
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }
    }
}
