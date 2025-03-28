import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { Employee } from '../../shared/models/employee';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ERoleType } from '../../shared/enums/e-role-type';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [HttpClientModule, CommonModule],
  templateUrl: './employee-list.component.html',
  styleUrl: './employee-list.component.scss',
})
export class EmployeeListComponent {
  private http = inject(HttpClient);

  public dataSource: Employee[] = [];
  public isLoading: boolean = true;
  public selectedPerson!: Employee;
  public roles = ERoleType;
  public currentUser = localStorage.getItem('userDocument');

  constructor(private router: Router) {}

  ngOnInit() {
    this.http
      .get<Employee[]>('http://localhost:55000/employee', {
        headers: {
          Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
        },
      })
      .subscribe(
        (response: Employee[]) => {
          this.dataSource = response;
          this.dataSource.forEach(element => {
            if (element.managerId) {
              console.log(element);
              element.managerName = this.dataSource.filter(x => x.id == element.managerId)[0].name;
            }
          });

          this.isLoading = false;
        },
        (error) => {
          localStorage.setItem('authenticated', 'false');
          alert(error.error.message);
          this.router.navigate(['/login']);
        }
      );
  }

  public redirectToAdd(): void {
    this.router.navigate(['/employee-edit']);
  }

  public redirectToEdit(employeeId: number): void {
    this.router.navigate(['/employee-edit', employeeId]);
  }

  public deleteEmployee(employeeId: number): void {
    this.http
      .delete(`http://localhost:55000/employee/${employeeId}`, {
        headers: {
          Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
        },
      })
      .subscribe(
        (response) => {
          window.location.reload();
        },
        (error) => {
          alert(error.error.message);
        }
      );
  }

  public logoff(): void {
    this.http
      .get(`http://localhost:55000/auth/revoke`, {
        headers: {
          Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
        },
      })
      .subscribe(
        (response) => {
          localStorage.clear();
          this.router.navigate(['/login']);
        },
        (error) => {
          alert(error.error.message);
        }
      );
  }
}
