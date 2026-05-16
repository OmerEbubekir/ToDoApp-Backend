using System;

namespace Shared.Core.Exceptions;


public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}