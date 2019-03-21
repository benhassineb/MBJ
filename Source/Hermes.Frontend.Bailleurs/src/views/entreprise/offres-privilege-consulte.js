import { inject } from 'aurelia-framework';
import { SampleService } from '../../services/sample-service';

@inject(SampleService)
export class OffresPrivilegeConsulte {

  typologieList;
  offres;

  constructor(service) {
    this._service = service;
  }

  activate() {
    this._service.getTypologie()
      .then(result => {
        this.typologieList = result;
      });
    this._service.getOffresPrivileges()
      .then(result => {
        this.offres = result;
      });
  }

}
