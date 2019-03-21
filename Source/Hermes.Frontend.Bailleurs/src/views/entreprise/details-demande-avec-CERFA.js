import { inject } from 'aurelia-framework';
import { SampleService } from '../../services/sample-service';


@inject(SampleService)
export class DetailsDemandeAvecCerfa {


  constructor(service) {
    this._service = service;
    this.demande;
    this.isDisabled = true;
    this.data = [];
    this.infos;
  }

  activate() {
    return this._service.getDetailsDemandeAvecCerfa()
      .then(result =>{
        this.data = result.data;
        this.infos = result.infosDemande;
      } );
  }

  fieldsetDisabled() {
    this.isDisabled = false;
  }

}
