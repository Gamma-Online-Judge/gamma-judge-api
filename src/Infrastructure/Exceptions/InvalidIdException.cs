namespace Infrastructure.Exceptions;

class InvalidIdException : Exception
{
    public InvalidIdException(string? id) : base($"The id: {id} is invalid")
    {
    }
}