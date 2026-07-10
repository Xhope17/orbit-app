import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-public-navbar-component',
  imports: [RouterLink],
  templateUrl: './public-navbar-component.html',
  styleUrl: './public-navbar-component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PublicNavbarComponent {}
