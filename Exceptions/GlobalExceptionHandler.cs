using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace InnoShop.UserService.Exceptions;

public class GlobalExceptionHandler()
	: IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		httpContext.Response.StatusCode = exception switch {
			NotFoundException => StatusCodes.Status404NotFound,
			UserExistsException => StatusCodes.Status409Conflict,
			_ => StatusCodes.Status500InternalServerError,
		};

		await httpContext.Response.WriteAsJsonAsync(new ProblemDetails {
			Type = exception.GetType().Name,
			Title = "An error occured",
			Detail = exception.Message,
		}, cancellationToken: cancellationToken);

		return true;
	}
}