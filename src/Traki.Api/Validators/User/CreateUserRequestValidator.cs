using FluentValidation;
using Traki.Api.Contracts.User;
using Traki.Api.Validators.Protocol;

namespace Traki.Api.Validators.User
{
    public class CreateUserRequestValidator: AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator() 
        {
            RuleFor(p => p.User).SetValidator(new UserValidator());
        }
    }
}
