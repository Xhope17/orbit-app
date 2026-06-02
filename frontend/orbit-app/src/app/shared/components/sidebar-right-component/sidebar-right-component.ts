import { ChangeDetectionStrategy, Component, inject, input, output, signal } from '@angular/core';
import { NavigationEnd, Router, RouterLink } from '@angular/router';
import { filter } from 'rxjs';

@Component({
  selector: 'app-sidebar-right',
  imports: [RouterLink],
  templateUrl: './sidebar-right-component.html',
  styleUrl: './sidebar-right-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarRightComponent {
  isAuthenticated = input<boolean>(false);
  username = input<string | null>(null);

  onLogout = output<void>();

  private router = inject(Router);

  // si estamos en la página de búsqueda
  public isSearchRoute = signal<boolean>(false);

  constructor() {
    // si el usuario recarga la página estando ya en /search
    this.isSearchRoute.set(this.router.url.includes('/search'));


    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.isSearchRoute.set(event.urlAfterRedirects.includes('/search'));
      });
  }

  onSearch(query: string) {
    if (query.trim()) {
      this.router.navigate(['/search'], { queryParams: { q: query.trim() } });
    }
  }
}
