using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Drawing.Defect;

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
