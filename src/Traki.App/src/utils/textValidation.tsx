import { error } from "console";

export const validationRules = {
  nonEmpty: {
    requirement: /\S+/,
    message: "Field cannot be empty"
  },
  noSpecialSymbols: {
    requirement: /^[a-zA-ZąčęėįšųūžĄČĘĖĮŠŲŪŽ0-9\s.,:;?!%()_&+={}[\]|\\/~$^+-]*$/,
    message: "Specials symbols are not allowed"
  }
};

export function validate(text: string, rules: ValidationRule[]): ValidationResult {
  for (let i = 0; i < rules.length; i++) {
    const rule = rules[i];
    const phoneResult = rule.requirement.test(text);
    if (!phoneResult) {
      return {invalid: true, message: rule.message};
    }
  }

  //updateErrorState(x => x && true);
  return {invalid: false, message: ''};
}


export type ValidationResult = {
  invalid: boolean
  message: string
}

export type ValidationRule = {
  requirement: RegExp,
  message: string
}