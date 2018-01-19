import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';

import { OAuthService } from 'angular-oauth2-oidc';
import { BaseService } from './base.service';
import { User } from '../models/user';
import { AppConfig } from '../appConfg';

@Injectable()
export class UserService extends BaseService {

  constructor(private oAuthService: OAuthService,
    private appConfig: AppConfig,
    private http: Http) {
    super(oAuthService);
  }

  getUserByEmail(email: string): Observable<User> {
    let reqOptions = this.getHttpRequestOptions();

    let url = this.appConfig.apiBaseUrl + 'users/email?email=' + email;

    return this.http.get(url, reqOptions)
        .map((response: Response) => response.json() as User);
  }

  createUser(user: User): Observable<User> {
    let reqOptions = this.getHttpRequestOptions();

    let url = this.appConfig.apiBaseUrl + 'users';

    let body = JSON.stringify(user).toString();

    return this.http.post(url, body, reqOptions)
      .map((response: Response) => response.json() as User);
  }
}
