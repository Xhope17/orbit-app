import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { UpperCasePipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Post } from '../../../features/interfaces/post.interface';

@Component({
  selector: 'app-post-card',
  standalone: true,
  imports: [UpperCasePipe, DatePipe, RouterLink],
  templateUrl: './post-card-component.html',
  styleUrl: './post-card-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PostCardComponent {
  post = input.required<Post>();
  currentUserId = input<string | null>(null);

  onDelete = output<string>();
  onLike = output<string>();
}
