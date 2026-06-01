import { ChangeDetectionStrategy, Component } from '@angular/core';
import { EnContruccionComponent } from "../../../../../shared/components/en-contruccion-component/en-contruccion-component";

@Component({
  selector: 'app-community-page',
  imports: [EnContruccionComponent],
  templateUrl: './community-page.html',
  styleUrl: './community-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommunityPage {}
