import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'localDate',
  standalone: true,
})
export class LocalDatePipe implements PipeTransform {
  transform(value: string | null | undefined, format: string = 'd MMM, h:mm a'): string {
    if (!value) return '';

    const normalized = value.endsWith('Z') || value.includes('+') ? value : value + 'Z';
    const date = new Date(normalized);

    if (isNaN(date.getTime())) return '';

    const options = this.getDateFormatOptions(format);
    return new Intl.DateTimeFormat(undefined, options).format(date);
  }

  private getDateFormatOptions(format: string): Intl.DateTimeFormatOptions {
    switch (format) {
      case 'short':
        return { dateStyle: 'short', timeStyle: 'short' };
      case 'medium':
        return { dateStyle: 'medium', timeStyle: 'medium' };
      case 'long':
        return { dateStyle: 'long', timeStyle: 'long' };
      case 'shortDate':
        return { dateStyle: 'short' };
      case 'shortTime':
        return { timeStyle: 'short' };
      case 'd MMM, h:mm a':
        return { day: 'numeric', month: 'short', hour: 'numeric', minute: '2-digit' };
      case 'd MMM, yyyy':
        return { day: 'numeric', month: 'short', year: 'numeric' };
      default:
        return { dateStyle: 'medium' };
    }
  }
}
