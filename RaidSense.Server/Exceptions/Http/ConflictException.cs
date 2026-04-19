using System;

namespace RaidSense.Server.Exceptions.Http;

public class ConflictException : BaseHttpException
{
    public ConflictException(string message) : base(message, 409) {}
}
