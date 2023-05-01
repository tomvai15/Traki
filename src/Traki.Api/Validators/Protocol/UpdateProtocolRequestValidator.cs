using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Protocol;

namespace Traki.Api.Validators.Protocol
{
    [ExcludeFromCodeCoverage]
    public class UpdateProtocolRequestValidator: AbstractValidator<UpdateProtocolRequest>
    {
        public UpdateProtocolRequestValidator() 
        {
            RuleFor(p => p.Protocol).SetValidator(new ProtocolValidator());
        }
    }
}
