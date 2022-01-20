namespace Infrastructure.Exceptions;

class IdAlreadyExists : Exception
{
    public IdAlreadyExists(string? id) : base($"The id: {id} already exists")
    {
    }
}