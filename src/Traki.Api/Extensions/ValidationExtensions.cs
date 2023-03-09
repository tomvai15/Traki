using Traki.Api.Exceptions;

namespace Traki.Api.Extensions
{
    public static class ValidationExtensions
    {
        public static void RequiresNotNullEnity(this object entity)
        {
            if (entity == null)
            {
                throw new EntityNotFoundException();
            }
        }
    }
}
