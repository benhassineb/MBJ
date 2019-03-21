import { inject } from 'aurelia-framework';
import { SampleService } from '../../services/sample-service';

@inject(SampleService)
export class DetailsDemandeSansCERFA {


  constructor(service) {
    this._service = service;
    this.demande;
  }

  activate(params) {
    if (params && params.id) this.id = params.id;

    return this._service.getDetailsDemandeSansCERFA()
      .then(result => this.demande = result);
  }

}
