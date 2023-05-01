using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Section;

namespace Traki.Api.Validators.Section
{
    [ExcludeFromCodeCoverage]
    public class UpdateSectionRequestValidator: AbstractValidator<UpdateSectionRequest>
    {
        public UpdateSectionRequestValidator() 
        {
            
        }
    }
}
