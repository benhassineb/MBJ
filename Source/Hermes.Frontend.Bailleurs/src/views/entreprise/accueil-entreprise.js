import { inject } from 'aurelia-framework';
import { SampleService } from 'services/sample-service';

@inject(SampleService)
export class AcceuilEntreprise {

  constructor(service) {
    this._service = service;
    this.names = [];
    this.offres = [];
  }

  activate(params, routeConfig, navigationInstruction) {
    this._service.getAcceuilEntrepriseData().then((res) => {
      this.names = res.names;
      this.offres = res.offres;
    });
  }

}
