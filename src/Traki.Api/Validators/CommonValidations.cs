using FluentValidation.Validators;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Traki.Api.Validators
{
    public static class CommonValidations
    {
        public static IRuleBuilderOptions<T, string> NoSpecialSymbols<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder.Matches("^[a-zA-ZąčęėįšųūžĄČĘĖĮŠŲŪŽ0-9\\s.,:;?!%()_&+={}\\[\\]|\\\\/~$^+-]*$").WithMessage("Specials symbols are not allowed");

        public static IRuleBuilderOptions<T, string> OnlyAlphabetSymbols<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder.Matches("^[a-zA-ZąčęėįšųūžĄČĘĖĮŠŲŪŽ]*$").WithMessage("Non alphabet symbols are not allowed");

        public static IRuleBuilderOptions<T, string> ValidFileName<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder.NotEmpty()
                .Must(BeAValidFileName)
                .WithMessage("Invalid file name");

        private static bool BeAValidFileName(string fileName)
        {
            string invalidChars = new string(Path.GetInvalidFileNameChars());
            string regexPattern = string.Format("^[^{0}]+$", Regex.Escape(invalidChars));
            return Regex.IsMatch(fileName, regexPattern);
        }
    }
}


