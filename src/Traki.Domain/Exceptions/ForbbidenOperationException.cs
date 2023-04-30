namespace Traki.Domain.Exceptions
{
    public class ForbbidenOperationException : Exception
    {
        public ForbbidenOperationException()
        {
        }

        public ForbbidenOperationException(string message)
            : base(message)
        {
        }

        public ForbbidenOperationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
