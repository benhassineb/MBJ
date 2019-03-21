import { inject } from 'aurelia-framework';
import { SampleService } from '../../services/sample-service';

@inject(SampleService)
export class DetailsDemandeSansCerfa {


  constructor(service) {
    this._service = service;
    this.demande;
  }

  activate() {
    return this._service.getDetailsDemandeSansCerfa()
      .then(result => this.demande = result);
  }

}
