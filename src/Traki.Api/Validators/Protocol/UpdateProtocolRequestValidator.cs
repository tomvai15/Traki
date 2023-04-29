using FluentValidation;
using Traki.Api.Contracts.Protocol;

namespace Traki.Api.Validators.Protocol
{
    public class UpdateProtocolRequestValidator: AbstractValidator<UpdateProtocolRequest>
    {
        public UpdateProtocolRequestValidator() 
        {
            RuleFor(p => p.Protocol).SetValidator(new ProtocolValidator());
        }
    }
}
