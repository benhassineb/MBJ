import {customElement, bindable, bindingMode } from 'aurelia-framework';
import { inject } from 'aurelia-framework';
import { SampleService } from 'services/sample-service';

@inject(SampleService)
@customElement('contact-privilegie')
export class contactprivilegie {
    
    @bindable({ defaultBindingMode: bindingMode.twoWay }) entreprise;
    @bindable filiale;

    constructor(service) {
        this._service = service;
        
    }

    entrepriseChanged(newEntreprise){
        this._service.getContactPriviligies(newEntreprise.id)
        .then(result => this.listContacts = result);
    }

}