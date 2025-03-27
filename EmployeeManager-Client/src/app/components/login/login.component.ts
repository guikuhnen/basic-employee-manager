import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { JsonPipe } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Token } from '../../shared/models/auth/token';
import { User } from '../../shared/models/auth/user';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    JsonPipe,
    RouterLink,
    HttpClientModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent implements OnInit {
  private http = inject(HttpClient);
  public loginForm: FormGroup = new FormGroup({});

  constructor(
    private router: Router,
    private fb: FormBuilder,
  ) {}

  ngOnInit() {
    this.loginForm = this.fb.group({
      document: ['', Validators.compose([Validators.required])],
      password: ['', Validators.required],
    });
  }

  onSubmit() {
    const { document, password } = this.loginForm.value;

    this.http.post<Token>('http://localhost:55000/auth/signin', {
      userDocument: document,
      password: password,
    } as User).subscribe((response: Token) => {
      localStorage.setItem('userDocument', document);
      localStorage.setItem('accessToken', response.accessToken);
      localStorage.setItem('authenticated', response.authenticated.toString());
      localStorage.setItem('refreshToken', response.refreshToken);

      this.router.navigate(['/employee-list']);
    }, (error) => {
      localStorage.setItem('authenticated', 'false');
      alert(error);
    });
  }
}
