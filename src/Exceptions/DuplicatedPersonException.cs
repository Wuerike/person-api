namespace PersonApi.Exceptions;
public class DuplicatedPersonException : Exception
{
    public DuplicatedPersonException(string? message) : base(message) { }
}
