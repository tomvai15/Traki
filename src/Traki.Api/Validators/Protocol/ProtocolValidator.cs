using FluentValidation;
using Traki.Api.Contracts.Protocol;

namespace Traki.Api.Validators.Protocol
{
    public class ProtocolValidator : AbstractValidator<ProtocolDto>
    {
        public ProtocolValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NoSpecialSymbols();
        }
    }
}
