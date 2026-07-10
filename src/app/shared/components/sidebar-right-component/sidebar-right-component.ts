import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  input,
  OnInit,
  output,
  signal,
} from '@angular/core';
import { NavigationEnd, Router, RouterLink } from '@angular/router';
import { filter } from 'rxjs';
import { TrendingService } from '../../../features/services/trending.service';
import { TrendingTopic } from '../../../features/interfaces/trending.interface';

@Component({
  selector: 'app-sidebar-right',
  imports: [RouterLink],
  templateUrl: './sidebar-right-component.html',
  styleUrl: './sidebar-right-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarRightComponent implements OnInit {
  isAuthenticated = input<boolean>(false);
  username = input<string | null>(null);

  onLogout = output<void>();

  private router = inject(Router);
  private trendingService = inject(TrendingService);

  // si estamos en la página de búsqueda
  public isSearchRoute = signal<boolean>(false);

  public trendingTopics = signal<TrendingTopic[]>([]);
  public displayLimit = signal(3);

  public visibleTopics = computed(() =>
    this.trendingTopics().slice(0, this.displayLimit()),
  );

  public hasMore = computed(() => this.trendingTopics().length > 3);

  constructor() {
    // si el usuario recarga la página estando ya en /search
    this.isSearchRoute.set(this.router.url.includes('/search'));


    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.isSearchRoute.set(event.urlAfterRedirects.includes('/search'));
      });
  }

  ngOnInit(): void {
    this.trendingService.getTrending().subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          this.trendingTopics.set(res.data);
        }
      },
    });
  }

  showMore(): void {
    this.displayLimit.set(10);
  }

  onTrendClick(name: string): void {
    this.router.navigate(['/search'], { queryParams: { q: `#${name}` } });
  }

  onSearch(query: string) {
    if (query.trim()) {
      this.router.navigate(['/search'], { queryParams: { q: query.trim() } });
    }
  }
}
