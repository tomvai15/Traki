using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Protocol;

namespace Traki.Api.Validators.Protocol
{
    [ExcludeFromCodeCoverage]
    public class ProtocolValidator : AbstractValidator<ProtocolDto>
    {
        public ProtocolValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NoSpecialSymbols();
        }
    }
}
