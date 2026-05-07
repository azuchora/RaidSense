using System;

namespace RaidSense.Server.Exceptions.Http;

public class UnauthorizedException : BaseHttpException
{
    public UnauthorizedException(string message) : base(message, 401) {}
}
