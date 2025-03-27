import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { Employee } from '../../shared/models/employee';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

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
  private displayedColumns: string[] = [
    'ID',
    'Name',
    'Email',
    'DocumentNumber',
    'PhoneNumbers',
    'Manager',
    'Role',
    'BirthDate',
    'Active',
    'Edit',
    'Delete',
  ];

  public isLoading: boolean = true;
  public selectedPerson!: Employee;

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
          this.isLoading = false;
        },
        (error) => {
          alert(error);
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
    this.http.delete(`http://localhost:55000/employee/${employeeId}`).subscribe(
      (response) => {
        this.router.navigate(['/employee-list']);
      },
      (error) => {
        alert(error);
      }
    );
  }
}
