import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidebarLeftComponent } from '../../../../shared/components/sidebar-left-component/sidebar-left-component';
import { SidebarRightComponent } from '../../../../shared/components/sidebar-right-component/sidebar-right-component';
import { AuthService } from '../../../../shared/services/auth.service';
@Component({
  selector: 'app-main-layout',
  imports: [RouterOutlet, SidebarLeftComponent, SidebarRightComponent],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MainLayout {
  authService = inject(AuthService);

  handleLogout() {
    this.authService.logout();
  }
}
