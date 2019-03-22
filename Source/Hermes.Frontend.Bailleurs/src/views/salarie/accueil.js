//import {computedFrom} from 'aurelia-framework';
import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { Auth } from './auth';

@inject(Auth, Router)
export class Accueil {

  heading = 'Où voulez vous habiter ?';
  locateWhere = '';
  locateWhat = '';
  locatePrice = '';
  constructor(auth, router) {
    this.auth = auth;
    this.router = router;
  }
  submit() {
    this.locateWhere = this.locateWhere;
    this.locateWhat = this.locateWhat;
    this.locatePrice = this.locatePrice;
    this.router.navigate('recherche');
  }

}
