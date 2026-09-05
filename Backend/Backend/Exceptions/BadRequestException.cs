namespace Backend.Exceptions
{
    /// <summary>
    /// The request is structurally valid but breaks a business rule that annotations
    /// cannot express - reviewing an agreement you were not party to, for instance.
    /// Controllers translate this into 400 Bad Request.
    /// </summary>
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
