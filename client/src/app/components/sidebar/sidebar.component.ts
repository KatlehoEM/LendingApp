import { Component } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  isOpen: boolean = false;

  constructor(public authService: AuthService) {}

  toggleSidebar(){
    this.isOpen = !this.isOpen;
  }

  logout() {
    this.authService.logOut();
  }
}
