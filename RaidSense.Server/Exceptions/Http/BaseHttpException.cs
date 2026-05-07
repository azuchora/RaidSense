using System;

namespace RaidSense.Server.Exceptions.Http;

public class BaseHttpException : Exception
{
    public int StatusCode { get; }

    public BaseHttpException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}
