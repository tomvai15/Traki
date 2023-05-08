namespace Traki.Domain.Exceptions
{
    public class BadOperationException : Exception
    {
        public BadOperationException()
        {
        }

        public BadOperationException(string message)
            : base(message)
        {
        }

        public BadOperationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
