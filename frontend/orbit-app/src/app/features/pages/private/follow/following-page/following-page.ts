import { ChangeDetectionStrategy, Component } from '@angular/core';
import { EnContruccionComponent } from "../../../../../shared/components/en-contruccion-component/en-contruccion-component";

@Component({
  selector: 'app-following-page',
  imports: [EnContruccionComponent],
  templateUrl: './following-page.html',
  styleUrl: './following-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FollowingPage {}
