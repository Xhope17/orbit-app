import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-public-footer-component',
  imports: [],
  templateUrl: './public-footer-component.html',
  styleUrl: './public-footer-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PublicFooterComponent {}
