import { ChangeDetectionStrategy, Component } from '@angular/core';
import { EnContruccionComponent } from "../../../../shared/components/en-contruccion-component/en-contruccion-component";

@Component({
  selector: 'app-bookmarks-page',
  imports: [EnContruccionComponent],
  templateUrl: './bookmarks-page.html',
  styleUrl: './bookmarks-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BookmarksPage {}
