import { inject } from 'aurelia-framework';
import { Auth } from '../auth';
import { Router } from 'aurelia-router';

@inject(Auth, Router)
export class CompleteSaisie {
  label='';
  constructor(auth, router) {
    this.message = 'Hello world';
    this.auth = auth;
    this.router = router;
  }
  goSearch() {
    this.auth.connect();
    this.router.navigate("rechercher");
  }
}
