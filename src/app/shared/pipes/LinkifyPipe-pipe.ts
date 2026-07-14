import { Pipe, PipeTransform, inject } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Pipe({
  name: 'linkifyPipe',
  standalone: true,
})
export class LinkifyPipe implements PipeTransform {
  // Sanitizer nativo de Angular para temas de seguridad
  private sanitizer = inject(DomSanitizer);

  transform(text: string | null | undefined): SafeHtml {
    if (!text) return '';

    // para detectar URLs que empiecen con http, https, o www.
    const urlRegex = /(https?:\/\/[^\s]+)|(www\.[^\s]+)/g;

    // reemplaza el texto plano por una etiqueta <a> con el link
    const linkedText = text.replace(urlRegex, (url) => {
      // Si el usuario solo escribió "www.host.com" se agrega el https://
      const href = url.startsWith('http') ? url : `https://${url}`;

      return `<a href="${href}" target="_blank" rel="noopener noreferrer" class="text-teal-400 hover:text-teal-300 hover:underline transition-colors">${url}</a>`;
    });

    // se le dice a angular que confíe en el link
    return this.sanitizer.bypassSecurityTrustHtml(linkedText);
  }
}
