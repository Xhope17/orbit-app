import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';

@Component({
  selector: 'feed-header-component',
  imports: [],
  templateUrl: './feed-header-component.html',
  styleUrl: './feed-header-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FeedHeaderComponent {
  // Recibimos los estados desde el padre (FeedPage)
  activeTab = input.required<'foryou' | 'following'>();
  isLoading = input.required<boolean>();
  canRefresh = input.required<boolean>();
  showCooldownWarning = input.required<boolean>();

  // Emitimos acciones hacia el padre
  onTabChange = output<'foryou' | 'following'>();
  onRefresh = output<void>();
}
