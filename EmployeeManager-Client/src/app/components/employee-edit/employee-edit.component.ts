import { CommonModule, DatePipe } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Employee } from '../../shared/models/employee';
import { PhoneNumber } from '../../shared/models/phone-number';

@Component({
  selector: 'app-employee-edit',
  standalone: true,
  imports: [HttpClientModule, CommonModule, ReactiveFormsModule],
  providers: [DatePipe],
  templateUrl: './employee-edit.component.html',
  styleUrl: './employee-edit.component.scss',
})
export class EmployeeEditComponent {
  private http = inject(HttpClient);

  public employeeId: number = 0;
  public employeeForm: FormGroup;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private datePipe: DatePipe
  ) {
    this.employeeForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', Validators.required],
      documentNumber: ['', Validators.required],
      phoneNumbers: [],
      managerId: [''],
      role: ['', Validators.required],
      birthDate: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  ngOnInit() {
    this.employeeId = Number(this.route.snapshot.paramMap.get('employeeId'));

    // EDIT
    if (this.employeeId > 0) {
      this.http
        .get<Employee>(`http://localhost:55000/employee/${this.employeeId}`, {
          headers: {
            Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
          },
        })
        .subscribe(
          (response: Employee) => {
            this.employeeForm.patchValue({
              firstName: response.firstName,
              lastName: response.lastName,
              email: response.email,
              documentNumber: response.documentNumber,
              phoneNumbers:
                response.phoneNumbers?.length > 0
                  ? response.phoneNumbers[0].number
                  : null,
              managerId: response.managerId,
              role: response.role,
              birthDate: this.datePipe.transform(
                response.birthDate,
                'yyyy-MM-dd'
              ),
              password: response.password,
            });
          },
          (error) => {
            alert(error.statusText);
          }
        );
    }
  }

  submit(form: FormGroup) {
    if (form.valid) {
      let employee = form.value as Employee;
      employee.active = true;
      employee.role = Number(form.controls['role'].value);
      employee.name = employee.firstName + ' ' + employee.lastName;

      // EDIT
      if (this.employeeId > 0) {
        employee.id = this.employeeId;
        //employee.phoneNumbers.forEach((phoneNumber) => {
        //  phoneNumber.employeeId = this.employeeId;
        //});
        
        // TODO
        employee.managerId = null;
        employee.phoneNumbers = [
          {
            employeeId: this.employeeId,
            number: form.controls['phoneNumbers'].value,
          } as PhoneNumber,
        ];

        this.http
          .put<Employee>(
            'http://localhost:55000/employee',
            employee,
            {
              headers: {
                Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
              },
            }
          )
          .subscribe(
            (response) => {
              this.router.navigate(['/employee-list']);
            },
            (error) => {
              console.log(error);
              alert(error.statusText);
            }
          );
      } else {
        // TODO
        employee.managerId = null;
        employee.phoneNumbers = [
          {
            employeeId: 0,
            number: form.controls['phoneNumbers'].value,
          } as PhoneNumber,
        ];

        this.http
          .post<Employee>('http://localhost:55000/employee', employee, {
            headers: {
              Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
            },
          })
          .subscribe(
            (response) => {
              this.router.navigate(['/employee-list']);
            },
            (error) => {
              alert(error.statusText);
            }
          );
      }
    }
  }
}
