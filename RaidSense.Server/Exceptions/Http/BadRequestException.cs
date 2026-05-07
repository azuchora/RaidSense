using System;

namespace RaidSense.Server.Exceptions.Http;

public class BadRequestException : BaseHttpException
{
    public BadRequestException(string message) : base(message, 400) {}
}
