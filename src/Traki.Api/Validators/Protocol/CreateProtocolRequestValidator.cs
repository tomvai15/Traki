using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Protocol;

namespace Traki.Api.Validators.Protocol
{
    [ExcludeFromCodeCoverage]
    public class CreateProtocolRequestValidator: AbstractValidator<CreateProtocolRequest>
    {
        public CreateProtocolRequestValidator() 
        {
            RuleFor(p => p.Protocol).SetValidator(new ProtocolValidator());
        }
    }
}
