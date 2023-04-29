using FluentValidation;
using Traki.Api.Contracts.User;

namespace Traki.Api.Validators.User
{
    public class UserValidator: AbstractValidator<UserDto>
    {
        public UserValidator() 
        {
            RuleFor(p => p.Name).NotEmpty().OnlyAlphabetSymbols();
            RuleFor(p => p.Email).NotEmpty().EmailAddress();
            RuleFor(p => p.Surname).NotEmpty().OnlyAlphabetSymbols();
            RuleFor(p => p.Status).NotEmpty().OnlyAlphabetSymbols();
        }
    }
}
