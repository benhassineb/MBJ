import { inject } from 'aurelia-framework';
import { Auth } from './auth';

@inject(Auth)
export class HeaderSalarie {     
  constructor(auth) {
    this.auth = auth;
  }

}
