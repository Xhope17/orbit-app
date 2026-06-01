import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { UpperCasePipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Post } from '../../../features/interfaces/post.interface';
import { LinkifyPipe } from '../../pipes/LinkifyPipe-pipe';

@Component({
  selector: 'app-post-card',
  standalone: true,
  imports: [UpperCasePipe, DatePipe, RouterLink, LinkifyPipe],
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
