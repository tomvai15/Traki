using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Infrastructure.Entities.Drawing;

namespace Traki.Api.Validators.Drawing.Defect
{
    [ExcludeFromCodeCoverage]
    public class StatusChangeValidator: AbstractValidator<StatusChangeDto>
    {
        public StatusChangeValidator() 
        {
        }
    }
}
