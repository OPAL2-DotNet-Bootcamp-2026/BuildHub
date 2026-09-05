namespace Backend.Exceptions
{
    /// <summary>
    /// A request that is well-formed but clashes with the current state of the data -
    /// a taken email, or deleting a record something else still depends on.
    /// Controllers translate this into 409 Conflict.
    /// </summary>
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {
        }
    }
}
