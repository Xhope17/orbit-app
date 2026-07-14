import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PublicNavbarComponent } from '../components/public-navbar-component/public-navbar-component';
import { PublicFooterComponent } from '../components/public-footer-component/public-footer-component';

@Component({
  selector: 'app-public-layout',
  imports: [RouterOutlet, PublicNavbarComponent, PublicFooterComponent],
  templateUrl: './public-layout.html',
  styleUrl: './public-layout.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PublicLayout {}
