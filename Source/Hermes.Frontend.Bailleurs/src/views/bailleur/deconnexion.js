import { Auth } from './auth';
import { inject } from 'aurelia-framework';

@inject(Auth)
export class Deconnexion {     
  constructor(auth) {
    auth.disconnect();
  }

}
