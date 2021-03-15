import { Injectable } from '@angular/core';

@Injectable()
export class Authentication {
  /**
   * Contains a method to check if user is currently logged in
   */
  constructor() {
  }
  public isAuthenticated(): boolean {
    return (!(window.localStorage['currentUser'] === undefined ||
      window.localStorage['currentUser'] === null ||
      window.localStorage['currentUser'] === 'null' ||
      window.localStorage['currentUser'] === 'undefined' ||
      window.localStorage['currentUser'] === ''));
  }
}
