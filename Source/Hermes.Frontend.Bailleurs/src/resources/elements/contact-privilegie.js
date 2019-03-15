import {customElement, bindable, bindingMode } from 'aurelia-framework';
import { inject } from 'aurelia-framework';
import { SampleService } from 'services/sample-service';

@inject(SampleService)
@customElement('contact-privilegie')
export class contactprivilegie {
    
    constructor(service) {
        this._service = service;
        this._service.getContactPriviligies()
        .then(result => this.listContacts = result)
    }
}