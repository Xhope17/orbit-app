import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-public-header-component',
  imports: [],
  templateUrl: './public-header-component.html',
  styleUrl: './public-header-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PublicHeaderComponent {}
