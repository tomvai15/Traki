using FluentValidation;
using Traki.Api.Contracts.Report;

namespace Traki.Api.Validators.Report
{
    public class GenerateReportRequestValidator: AbstractValidator<GenerateReportRequest>
    {
        public GenerateReportRequestValidator() 
        {
            RuleFor(p => p.ReportTitle).NotEmpty().MaximumLength(50).NoSpecialSymbols();
        }
    }
}
