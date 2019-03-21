import { inject } from 'aurelia-framework';
import { Auth } from './auth';
import { Router } from 'aurelia-router';

@inject(Auth, Router)
export class InscriptionNud {     
  nud;
  hasNud;
  constructor(auth, router) {
    this.auth = auth;
    this.router = router;
    this.auth.connect();
  }
  submit() {
    this.router.navigate("saisie-demande");
  }
}
