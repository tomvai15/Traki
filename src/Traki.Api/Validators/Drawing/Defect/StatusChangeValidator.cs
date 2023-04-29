using FluentValidation;
using Traki.Infrastructure.Entities.Drawing;

namespace Traki.Api.Validators.Drawing.Defect
{
    public class StatusChangeValidator: AbstractValidator<StatusChangeDto>
    {
        public StatusChangeValidator() 
        {
        }
    }
}
