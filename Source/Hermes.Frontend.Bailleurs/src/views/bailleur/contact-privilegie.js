import { customElement, bindable, bindingMode } from 'aurelia-framework';
import { inject } from 'aurelia-framework';
import { SampleService } from 'services/sample-service';

@inject(SampleService)
export class ContactPrivilegie {

  constructor(service) {
    this._service = service;
  }

  activate(entreprise) {
    if (entreprise) {
      this._service.getContactPriviligies(entreprise.id)
        .then(result => this.listContacts = result);
    }
  }
}
