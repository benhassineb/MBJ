import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { Auth } from './auth';

@inject(Auth, Router)
export class Acceuil {
  constructor(auth, router) {
    this.auth = auth;
    this.router = router;
    if(!auth.authIsLoggedIn) {
      this.router.navigate('connexion');
    }
  }
}
