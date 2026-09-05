namespace Backend.Exceptions
{
    /// <summary>
    /// The caller is signed in, but this particular record is not theirs - editing
    /// someone else's job, or offering as a vendor they do not own. A role check
    /// cannot catch this: it depends on the row, not the role.
    /// Controllers translate this into 403 Forbidden.
    /// </summary>
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message)
        {
        }
    }
}
