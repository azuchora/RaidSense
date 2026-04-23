using System;

namespace RaidSense.Server.Exceptions.Http;

public class ForbiddenException : BaseHttpException
{
    public ForbiddenException(string message) : base(message, 403) {}
}
