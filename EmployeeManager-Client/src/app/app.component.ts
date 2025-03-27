import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  private http = inject(HttpClient);
  
  constructor(private router: Router) {}

  ngOnInit() {
    this.router.navigate(['/login']);
  }

  public logoff(): void {
    this.http.get(`http://localhost:55000/auth/revoke`, {
      headers: {
        Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
      },
    }).subscribe(
      (response) => {
        localStorage.clear();
        this.router.navigate(['/login']);
      },
      (error) => {
        alert(error);
      }
    );
  }
}
