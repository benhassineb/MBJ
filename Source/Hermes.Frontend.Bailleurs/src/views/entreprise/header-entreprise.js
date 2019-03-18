import { inject } from 'aurelia-framework';
import { SampleService } from 'services/sample-service';
import {customElement, bindable, bindingMode } from 'aurelia-framework';

@inject(SampleService)
@customElement('header-entreprise')
export class headerentreprise {

    listFiltreEntreprise;
    listFiltreEntrepriseSelected = [];
    
    constructor(service) {
      this._service = service;
      this._service.getFiltreEntreprise()
        .then(result => {this.listFiltreEntreprise = result; this.listFiltreEntrepriseSelected.push(this.listFiltreEntreprise[1]);  });
    }

    removeFiltreEntreprise(filtre) {
      this.listFiltreEntrepriseSelected = this.listFiltreEntrepriseSelected.filter(item => item.code !== filtre.code);
    }
}