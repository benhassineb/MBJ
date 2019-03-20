import { inject } from 'aurelia-framework';
import { SampleService } from '../../services/sample-service';


@inject(SampleService)
export class DetailsDemandeAvecCERFA {


  constructor(service) {
    this._service = service;
    this.demande;
    this.isDisabled = true;
  }

  activate() {
    return this._service.getDetailsDemandeAvecCERFA()
      .then(result => this.demande = result);
  }

  fieldsetDisabled() {  
    this.isDisabled = false;
  }

}
