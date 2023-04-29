using FluentValidation;
using Traki.Api.Contracts.Protocol;
using Traki.Api.Validators.Project;

namespace Traki.Api.Validators.Protocol
{
    public class CreateProtocolRequestValidator: AbstractValidator<CreateProtocolRequest>
    {
        public CreateProtocolRequestValidator() 
        {
            RuleFor(p => p.Protocol).SetValidator(new ProtocolValidator());
        }
    }
}
