import { inject } from 'aurelia-framework';
import { SampleService } from 'services/sample-service';

@inject(SampleService)
export class AcceuilSalarie {

  constructor(service) {
    this._service = service;
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
