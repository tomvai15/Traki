using Traki.Api.Exceptions;

namespace Traki.Api.Extensions
{
    public static class ValidationExtensions
    {
        public static void RequiresToBeNotNullEnity(this object entity, Exception exception)
        {
            if (entity == null)
            {
                throw exception;
            }
        }
        public static void RequiresToBeNotNullEnity(this object entity)
        {
            entity.RequiresToBeNotNullEnity(new EntityNotFoundException());
        }
    }
}
