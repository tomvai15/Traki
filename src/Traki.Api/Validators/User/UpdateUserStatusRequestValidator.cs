using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.User;

namespace Traki.Api.Validators.User
{
    [ExcludeFromCodeCoverage]
    public class UpdateUserStatusRequestValidator: AbstractValidator<UpdateUserStatusRequest>
    {
        public UpdateUserStatusRequestValidator() 
        {
            RuleFor(p => p.Status).NotNull().NoSpecialSymbols().NotEmpty();
        }
    }
}
