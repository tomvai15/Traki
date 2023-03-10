export function notEmptyOrNull( params: string[]): boolean {
  return params.some(x=> x == null || x == '');
}