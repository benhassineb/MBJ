import { Auth } from '../auth';
import { Router } from 'aurelia-router';
import { inject } from 'aurelia-framework';

@inject(Auth)
export class Deconnexion {     
  constructor(auth) {
    auth.disconnect();
  }

}
