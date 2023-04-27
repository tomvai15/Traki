export function formatInitials(name?: string, surname?: string): string {
  return (name ? name.toUpperCase()[0] : '') + ( surname ? surname.toUpperCase()[0] : '');
}