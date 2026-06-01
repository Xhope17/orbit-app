import { ChangeDetectionStrategy, Component } from '@angular/core';
import { EnContruccionComponent } from "../../../../../shared/components/en-contruccion-component/en-contruccion-component";

@Component({
  selector: 'app-followers-page',
  imports: [EnContruccionComponent],
  templateUrl: './followers-page.html',
  styleUrl: './followers-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FollowersPage {}
