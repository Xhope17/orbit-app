import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const RESERVED_USERNAMES = [
  'home', 'notifications', 'messages', 'explore', 'settings',
  'premium', 'post', 'auth', 'public', 'admin', 'api', 'i',
  'about', 'contact', 'feed', 'orbit', 'help', 'support',
  'login', 'register', 'logout', 'search', 'trends', 'bookmarks',
];

export function forbiddenUsernameValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value?.toLowerCase().trim();
    if (value && RESERVED_USERNAMES.includes(value)) {
      return { forbiddenUsername: 'Este nombre de usuario no está disponible' };
    }
    return null;
  };
}
