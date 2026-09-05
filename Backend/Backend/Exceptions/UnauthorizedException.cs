namespace Backend.Exceptions
{
    /// <summary>
    /// The caller has not proved who they are - bad credentials, or no usable token.
    /// Controllers translate this into 401 Unauthorized.
    /// </summary>
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}
