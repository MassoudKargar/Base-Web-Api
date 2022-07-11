namespace Base.Infrastructure.Exceptions;

public class NullSmpleException : AppException
{

    public NullSmpleException()
        : base(ApiResultStatusCode.NotExtended, System.Net.HttpStatusCode.NotExtended)
    {
    }

    public NullSmpleException(string message)
        : base(ApiResultStatusCode.NotExtended, message, System.Net.HttpStatusCode.NotExtended)
    {
    }

    public NullSmpleException(object additionalData)
        : base(ApiResultStatusCode.NotExtended, null, System.Net.HttpStatusCode.NotExtended, additionalData)
    {
    }

    public NullSmpleException(string message, object additionalData)
        : base(ApiResultStatusCode.NotExtended, message, System.Net.HttpStatusCode.NotExtended, additionalData)
    {
    }

    public NullSmpleException(string message, Exception exception)
        : base(ApiResultStatusCode.NotExtended, message, exception, System.Net.HttpStatusCode.NotExtended)
    {
    }

    public NullSmpleException(string message, Exception exception, object additionalData)
        : base(ApiResultStatusCode.NotExtended, message, System.Net.HttpStatusCode.NotExtended, exception, additionalData)
    {
    }
}