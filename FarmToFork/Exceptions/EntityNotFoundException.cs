using System.Runtime.Serialization;

namespace FarmToFork.Exceptions;

public class EntityNotFoundException:Exception
{
    private readonly static string _message = "Entity not found";
    public EntityNotFoundException():base(_message)
    {
    }

    protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public EntityNotFoundException(string? message) : base(message)
    {
    }

    public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}