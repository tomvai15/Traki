using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.User;

namespace Traki.Api.Validators.User
{
    [ExcludeFromCodeCoverage]
    public class CreateUserRequestValidator: AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator() 
        {
            RuleFor(p => p.User).SetValidator(new UserValidator());
        }
    }
}
