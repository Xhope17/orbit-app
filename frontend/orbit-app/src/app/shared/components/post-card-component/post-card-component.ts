import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  inject,
  input,
  OnDestroy,
  output,
} from '@angular/core';
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
export class PostCardComponent implements OnDestroy {
  private el = inject(ElementRef);
  post = input.required<Post>();
  currentUserId = input<string | null>(null);

  onDelete = output<string>();
  onLike = output<string>();
  //guardar en favoritos
  onSave = output<string>();

  ngOnDestroy() {
    const videos = this.el.nativeElement.querySelectorAll('video');
    videos.forEach((video: HTMLVideoElement) => {
      video.pause();
      video.removeAttribute('src');
      video.load();
    });
  }
}
