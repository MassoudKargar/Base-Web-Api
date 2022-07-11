namespace Base.WebApi.Configuration.Middleware;

public static class CustomExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}

public class CustomExceptionHandlerMiddleware
{
    private RequestDelegate Next { get; }
    private IWebHostEnvironment Env { get; }
    private ILogger<CustomExceptionHandlerMiddleware> Logger { get; }

    public CustomExceptionHandlerMiddleware(RequestDelegate next,
        IWebHostEnvironment env,
        ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        Next = next;
        Env = env;
        Logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        string message = null;
        HttpStatusCode httpStatusCode = HttpStatusCode.ServiceUnavailable;
        ApiResultStatusCode apiStatusCode = ApiResultStatusCode.ServiceUnavailable;

        try
        {
            await Next(context);
            //WriteToLogger(new AppException());
        }
        catch (TimeoutException exception)
        {
            WriteToLogger(exception);
            SetResponse(exception, HttpStatusCode.RequestTimeout, ApiResultStatusCode.RequestTimeout);
            await WriteToResponseAsync();
        }
        catch (TaskCanceledException exception)
        {
            WriteToLogger(exception);
            SetResponse(exception, HttpStatusCode.GatewayTimeout, ApiResultStatusCode.GatewayTimeout);
            await WriteToResponseAsync();
        }
        catch (OperationCanceledException exception)
        {
            WriteToLogger(exception);
            SetResponse(exception, HttpStatusCode.Gone, ApiResultStatusCode.Gone);
            await WriteToResponseAsync();
        }
        catch (NotFoundException exception)
        {
            WriteToLogger(exception);
            SetResponse(exception, HttpStatusCode.NotFound, ApiResultStatusCode.NotFound);
            await WriteToResponseAsync();
        }
        catch (LogicException exception)
        {
            WriteToLogger(exception);
            SetResponse(exception, HttpStatusCode.Conflict, ApiResultStatusCode.Conflict);
            await WriteToResponseAsync();
        }
        catch (SqlException exception)
        {
            WriteToLogger(exception);
            SetResponse(exception, HttpStatusCode.ServiceUnavailable, ApiResultStatusCode.ServiceUnavailable);
            await WriteToResponseAsync();
        }
        catch (DatabaseExceptions exception)
        {
            WriteToLogger(exception);
            SetResponse(exception, exception.HttpStatusCode, exception.ApiStatusCode);
            await WriteToResponseAsync();
        }
        catch (BadRequestException exception)
        {
            WriteToLogger(exception);
            SetResponse(exception, HttpStatusCode.BadRequest, ApiResultStatusCode.BadRequest);
            await WriteToResponseAsync();
        }
        catch (UnauthorizedException exception)
        {
            WriteToLogger(exception);
            SetResponse(exception, HttpStatusCode.Unauthorized, ApiResultStatusCode.Unauthorized);
            await WriteToResponseAsync();
        }
        catch (SecurityTokenExpiredException exception)
        {
            WriteToLogger(exception);
            SetResponse(exception, HttpStatusCode.ExpectationFailed, ApiResultStatusCode.ExpectationFailed);
            await WriteToResponseAsync();
        }
        catch (UnauthorizedAccessException exception)
        {
            WriteToLogger(exception);
            SetResponse(exception, HttpStatusCode.Unused, ApiResultStatusCode.Unused);
            await WriteToResponseAsync();
        }
        catch (NullSmpleException exception)
        {
            WriteToLogger(exception);
            SetResponse(exception, HttpStatusCode.NotModified, ApiResultStatusCode.NullSampleException);
            await WriteToResponseAsync();
        }
        catch (AccessException exception)
        {
            WriteToLogger(exception);
            SetResponse(exception, HttpStatusCode.NotAcceptable, ApiResultStatusCode.NotAcceptable);
            await WriteToResponseAsync();
        }

        //catch (AppException exception)
        //{
        //    WriteToLogger(exception);
        //    SetResponse(exception, exception.HttpStatusCode, exception.ApiStatusCode);
        //    await WriteToResponseAsync();
        //}
        catch (Exception exception)
        {
            if (Env.IsDevelopment())
            {
                Dictionary<string, string> dic = new()
                {
                    ["Exception"] = exception.Message,
                    ["StackTrace"] = exception.StackTrace,
                };
                message = ServiceSerialize.JsonSerialize(dic);
            }

            WriteToLogger(exception);
            await WriteToResponseAsync();
        }
        finally
        {

        }

        void WriteToLogger(Exception exception)
        {
            Claim computerName = null;
            Claim ip = null;
            Claim roleId = null;
            Claim personRoleId = null;
            string body = "";
            string method = "";
            PathString path = "";
            try
            {
                context.User.Claims.ForAll(f =>
                {
                    switch (f.Type)
                    {
                        case UserClaimName.ComputerName: computerName = f; break;
                        case UserClaimName.Ip: ip = f; break;
                        case UserClaimName.RoleId: roleId = f; break;
                        case UserClaimName.PersonRoleId: personRoleId = f; break;
                    }
                });
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                using StreamReader stream = new(context.Request.Body);
                body = stream.ReadToEnd();
                method = context.Request.Method;
                path = context.Request.Path;
                if (roleId != null && personRoleId != null)
                {
                    Logger.LogError(exception, $"{ip}|{computerName}|{personRoleId}|{roleId}|method:{method}|path:{path}|Request({body})|EC(^^^{exception}^^^)|{exception.Message ?? ""}");
                }
            }
            catch
            {
                if (roleId != null && personRoleId != null)
                {
                    Logger.LogError(exception, $"{ip}|{computerName}|{personRoleId}|{roleId}|method:{method}|path:{path}|EC(^^^{exception}^^^)|Request({body})|{exception.Message ?? ""}");
                }
            }

        }
        async Task WriteToResponseAsync()
        {
            if (context.Response.HasStarted)
                throw new InvalidOperationException("پاسخ قبلا شروع شده است، میان افزار کد وضعیت http اجرا نخواهد شد.");
            context.Response.StatusCode = (int)httpStatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(ServiceSerialize.JsonSerialize(new ApiResult(false, apiStatusCode, message)));
        }

        void SetResponse(Exception exception, HttpStatusCode httpStatus, ApiResultStatusCode apiResultStatus)
        {
            httpStatusCode = httpStatus;
            apiStatusCode = apiResultStatus;

            if (Env.IsDevelopment())
            {
                var dic = new Dictionary<string, string>
                {
                    ["Exception"] = exception.Message,
                    ["StackTrace"] = exception.StackTrace
                };
                if (exception is SecurityTokenExpiredException tokenException)
                    dic.Add("Expires", tokenException.Expires.ToString());

                message = ServiceSerialize.JsonSerialize(dic);
            }
        }
    }
}
