import { inject } from 'aurelia-framework';
import { Auth } from './auth';
import { Router } from 'aurelia-router';

@inject(Auth, Router)
export class Connexion {
  mail = '';
  password = '';
  constructor(auth, router) {
    this.auth = auth;
    this.router = router;
  }
  submit() {
    //App.connect();
    //window.location.href = '/recherche';
    this.auth.connect();
    this.router.navigate("accueil");
  }
}
