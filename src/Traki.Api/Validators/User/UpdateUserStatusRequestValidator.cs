using FluentValidation;
using Traki.Api.Contracts.User;

namespace Traki.Api.Validators.User
{
    public class UpdateUserStatusRequestValidator: AbstractValidator<UpdateUserStatusRequest>
    {
        public UpdateUserStatusRequestValidator() 
        {
            RuleFor(p => p.Status).NotNull().NoSpecialSymbols().NotEmpty();
        }
    }
}
