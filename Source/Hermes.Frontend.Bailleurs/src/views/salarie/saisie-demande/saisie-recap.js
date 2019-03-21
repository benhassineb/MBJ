import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';

@inject(Router)
export class SaisieRecap {
  label = 'Recap';
  constructor(router) {
    this.entreprise = 'Orange';
    this.router = router;
  }

}
