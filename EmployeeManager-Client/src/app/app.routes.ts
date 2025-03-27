import { Routes } from '@angular/router';
import { EmployeeListComponent } from './components/employee-list/employee-list.component';
import { EmployeeEditComponent } from './components/employee-edit/employee-edit.component';
import { LoginComponent } from './components/login/login.component';
import { authGuard } from './components/guard/auth.guard';

export const routes: Routes = [
    {
        path: 'login',
        component: LoginComponent,
        canActivate: [authGuard]
    },
    {
        path: 'employee-list',
        component: EmployeeListComponent,
        canActivate: [authGuard]
    },
    {
        path: 'employee-edit',
        component: EmployeeEditComponent,
        canActivate: [authGuard]
    },
    {
        path: 'employee-edit/:employeeId',
        component: EmployeeEditComponent,
        canActivate: [authGuard]
    }
];
