import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SearchPostComponent } from '../components/search-post-component/search-post-component';
import { SearchProfileComponent } from '../components/search-profile-component/search-profile-component';

@Component({
  selector: 'app-search-page',
  standalone: true,
  imports: [SearchPostComponent, SearchProfileComponent],
  templateUrl: './search-page.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SearchPage implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  public query = signal<string>('');
  public activeTab = signal<'posts' | 'personas'>('posts');

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      const q = params['q'] || '';
      this.query.set(q);
    });
  }

  setTab(tab: 'posts' | 'personas') {
    this.activeTab.set(tab);
  }

  // buscador central
  onSearch(searchTerm: string) {
    const term = searchTerm.trim();
    if (term) {
      this.router.navigate(['/search'], { queryParams: { q: term } });
    } else {
      this.router.navigate(['/search']); //limpiamos la URL
    }
  }
}
