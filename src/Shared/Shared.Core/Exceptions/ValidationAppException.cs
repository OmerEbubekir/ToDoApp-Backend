namespace Shared.Core.Exceptions;

public class ValidationAppException : Exception
{
    public List<string> Errors { get; }

    public ValidationAppException(List<string> errors) : base("Doğrulama hatası meydana geldi.")
    {
        Errors = errors;
    }
}