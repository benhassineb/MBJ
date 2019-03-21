import { inject } from 'aurelia-framework';
import { Auth } from './auth';
import { Router } from 'aurelia-router';

@inject(Auth, Router)
export class Inscription {
  mail;
  password;
  civilite;
  nom;
  prenom;
  portable;
  politique;
  contrat; 
  constructor(auth, router) {
    this.auth = auth;
    this.router = router;
  }
  submit() {
    this.auth.connect();
    this.router.navigate("inscription-nud");
  }
}

