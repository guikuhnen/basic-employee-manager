import { CommonModule, DatePipe } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Employee } from '../../shared/models/employee';
import { PhoneNumber } from '../../shared/models/phone-number';
import { ERoleType } from '../../shared/enums/e-role-type';

@Component({
  selector: 'app-employee-edit',
  standalone: true,
  imports: [HttpClientModule, CommonModule, ReactiveFormsModule, FormsModule],
  providers: [DatePipe],
  templateUrl: './employee-edit.component.html',
  styleUrl: './employee-edit.component.scss',
})
export class EmployeeEditComponent {
  private http = inject(HttpClient);

  public employeeId: number = 0;
  public employeeForm: FormGroup;
  public roleSelectList = new Map<number, string>();
  public managerSelectList = new Map<number, string>();

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
      phoneNumbers: [''],
      managerId: [''],
      role: ['', Validators.required],
      birthDate: ['', Validators.required],
      password: ['', Validators.required],
    });

    for (let index = 0; index < Object.keys(ERoleType).length / 2; index++) {
      this.roleSelectList.set(index, ERoleType[index]);
    }
  }

  ngOnInit() {
    this.employeeId = Number(this.route.snapshot.paramMap.get('employeeId'));

    this.getManagersToSelectList();

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
                  ? response.phoneNumbers.map((x) => x.number).join(', ')
                  : null,
              managerId: response.managerId === null ? '' : response.managerId,
              role: response.role,
              birthDate: this.datePipe.transform(
                response.birthDate,
                'yyyy-MM-dd'
              ),
              password: response.password,
            });
          },
          (error) => {
            alert(error.error.message);
          }
        );
    }
  }

  public submit(form: FormGroup) {
    if (form.valid) {
      let employee = form.value as Employee;
      employee = this.formatEmployeeForSubmit(employee, form);

      // EDIT
      if (this.employeeId > 0) {
        employee.id = this.employeeId;
        employee.phoneNumbers.forEach((phoneNumber) => {
          phoneNumber.employeeId = this.employeeId;
        });

        this.http
          .put<Employee>('http://localhost:55000/employee', employee, {
            headers: {
              Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
            },
          })
          .subscribe(
            (response) => {
              this.router.navigate(['/employee-list']);
            },
            (error) => {
              alert(error.error.message);
            }
          );
      } else {
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
              alert(error.error.message);
            }
          );
      }
    }
  }

  private getManagersToSelectList() {
    this.http
      .get<Employee[]>('http://localhost:55000/employee', {
        headers: {
          Authorization: 'Bearer ' + localStorage.getItem('accessToken'),
        },
      })
      .subscribe(
        (response: Employee[]) => {
          response.forEach((employee) => {
            this.managerSelectList.set(
              employee.id,
              employee.name ?? employee.firstName
            );
          });
        },
        (error) => {
          alert(error.error.message);
        }
      );
  }

  private formatEmployeeForSubmit(
    employee: Employee,
    form: FormGroup
  ): Employee {
    employee.active = true;
    employee.role = Number(form.controls['role'].value);

    employee.name = employee.firstName + ' ' + employee.lastName;
    if (employee.managerId?.toString() === '') {
      employee.managerId = null;
    }

    const phones: string[] = form.controls['phoneNumbers'].value
      .replace(/\s/g, '')
      .split(',');
    employee.phoneNumbers = [];
    phones?.forEach((element) => {
      if (Number(element)) {
        employee.phoneNumbers.push({
          employeeId: 0,
          number: element,
        } as PhoneNumber);
      }
    });

    return employee;
  }
}
