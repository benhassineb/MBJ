import { inject } from 'aurelia-framework';
import { Auth } from './auth';

@inject(Auth)
export class Header {     
  constructor(auth) {
    this.auth = auth;
  }

}
