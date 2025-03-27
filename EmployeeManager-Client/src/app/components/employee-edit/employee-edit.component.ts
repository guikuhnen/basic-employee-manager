import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Employee } from '../../shared/models/employee';

@Component({
  selector: 'app-employee-edit',
  standalone: true,
  imports: [HttpClientModule, CommonModule],
  templateUrl: './employee-edit.component.html',
  styleUrl: './employee-edit.component.scss',
})
export class EmployeeEditComponent {
  private http = inject(HttpClient);
  private employee: number = 0;

  constructor(private router: Router, private route: ActivatedRoute) {}

  ngOnInit() {
    this.employee = Number(this.route.snapshot.paramMap.get('employeeId'));

    console.log(this.employee);
  }
}
