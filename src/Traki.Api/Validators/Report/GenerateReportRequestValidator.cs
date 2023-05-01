using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Report;

namespace Traki.Api.Validators.Report
{
    [ExcludeFromCodeCoverage]
    public class GenerateReportRequestValidator: AbstractValidator<GenerateReportRequest>
    {
        public GenerateReportRequestValidator() 
        {
            RuleFor(p => p.ReportTitle).NotEmpty().MaximumLength(50).NoSpecialSymbols();
        }
    }
}
