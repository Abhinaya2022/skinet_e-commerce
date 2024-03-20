import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account.service';
import { User } from 'src/app/shared/models/user';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', Validators.required),
  });

  constructor(private service: AccountService, private router: Router) {}

  ngOnInit(): void {}

  onSubmit() {
    if (this.loginForm.invalid) return;
    this.service.login(this.loginForm.value).subscribe({
      next: () => this.router.navigateByUrl('/shop'),
    });
  }
}
