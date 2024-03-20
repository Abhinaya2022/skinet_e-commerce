import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { User } from '../shared/models/user';
import { BehaviorSubject, Observable, map, of } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl + 'Account/';
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();
  private loggedInSource = new BehaviorSubject<boolean>(false);
  loggedIn$ = this.loggedInSource.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  isValidEmail(email: string) {
    let params = new HttpParams();
    if (email) params = params.append('email', email);
    return this.http.get(this.baseUrl + 'emailexists', {
      params: params,
    });
  }

  loadCurrentUser(token: string) {
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.get<User>(this.baseUrl, { headers: headers }).pipe(
      map((user) => {
        localStorage.setItem('token', user.token);
        this.currentUserSource.next(user);
      })
    );
  }

  login(loginDto: any) {
    return this.http.post<User>(this.baseUrl + 'login', loginDto).pipe(
      map((user: any) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(values: any) {
    return this.http.post<User>(this.baseUrl + 'register', values).pipe(
      map((user: any) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/home');
  }

  checkEmailExists(email: string) {
    let params = new HttpParams();
    if (email) params = params.append('email', email);

    return this.http.get<boolean>(this.baseUrl + 'emailExists', {
      params: params,
    });
  }
}
