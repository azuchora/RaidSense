using System;

namespace RaidSense.Server.Exceptions.Http;

public class NotFoundException : BaseHttpException
{
    public NotFoundException(string message) : base(message, 404) {}
}
