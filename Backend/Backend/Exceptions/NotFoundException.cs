namespace Backend.Exceptions
{
    /// <summary>
    /// A referenced record does not exist - a bad foreign key in the request body,
    /// for instance. Controllers translate this into 404 Not Found, which is the point
    /// of checking: a bad id must not surface as a raw foreign-key 500.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
