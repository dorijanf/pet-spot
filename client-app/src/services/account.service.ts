import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppConfig } from '../app/configuration/config'
import { AuthorizeBm } from '../app/generated-models/authorize-bm';
import { AuthorizeResponseDto } from '../app/generated-models/authorize-response-dto';
import { map } from 'rxjs/operators';
import { RegisterUserBm } from 'src/app/generated-models/register-user-bm';

@Injectable({
  providedIn: 'root'
})

export class AccountService {
  private currentUserSubject: BehaviorSubject<AuthorizeResponseDto>;
  public currentUser: Observable<AuthorizeResponseDto>;
  apiUrl: string;
  appUrl: string;

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8'
    })
  };

  /**
   * Service containing methods for user
   * creation and authorization.
   * @param http
   * @param config
   */
  constructor(private http: HttpClient, private config: AppConfig) {
    this.currentUserSubject = new BehaviorSubject<AuthorizeResponseDto>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
    this.apiUrl = this.config.setting['PathAPI'];
    this.appUrl = '/api/Account/'
  }

  /**
   * Authorization method that takes in username and password
   * and stores the user in local storage.
   * @param model - username and password
   * @returns AuthorizeResponseDto containing UserName
   */
  login(model: AuthorizeBm) {
    return this.http.post<AuthorizeResponseDto>(this.apiUrl + this.appUrl + 'login',
      model).pipe(map(user => {
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        return user;
      }));
  }

  /**
  * Removes the user from local storage to log user out.
  */
  logout() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  /**
   * Creates a new user
   * @param model - user registration information
   * @returns
   */
  register(model: RegisterUserBm) {
    return this.http.post(this.apiUrl + this.appUrl + 'register', model);
  }

  /**
   * Used for retrieval of information about current user.
   * Mainly used by auth guard.
   * @returns Information about currently logged in user
   */
  public get currentUserValue(): AuthorizeResponseDto {
    return this.currentUserSubject.value;
  }
}
