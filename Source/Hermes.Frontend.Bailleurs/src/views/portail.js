import { SampleService } from 'services/sample-service';
import { Router } from 'aurelia-router';
import { inject } from 'aurelia-framework';

@inject(SampleService, Router)
export class Portail {

  constructor(service, router) {
    this._service = service;
    this.router = router;

    this._service.getLogements()
      .then(result => this.logements = result);
    this._service.getCommunes()
      .then(result => this.communes = result);
    this._service.getReseauFerre()
      .then(result => this.reseauferre = result);
  }

  effacerCacheAutorisations() {
    return this._service.effacerCacheAutorisations('test')
      .then(result => this.result = result);
  }

}
