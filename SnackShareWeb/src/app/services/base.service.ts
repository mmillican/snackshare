import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { OAuthService } from 'angular-oauth2-oidc';

export class BaseService {

  constructor(private _oAuthService: OAuthService) { }

  getHttpRequestOptions(): RequestOptions {
    let headers = new Headers({ 'Authorization': 'Bearer ' + this._oAuthService.getAccessToken(), 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    
    return options;
  }

}
