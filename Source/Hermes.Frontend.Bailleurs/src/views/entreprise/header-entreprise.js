import { inject } from 'aurelia-framework';
import { SampleService } from 'services/sample-service';
import {customElement, bindable, bindingMode } from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {AutoCompleteController} from 'aurelia-autocomplete';

@inject(SampleService, Router)
@customElement('header-entreprise')
export class headerentreprise {

    listDepartement;
    listFiltreEntreprise;
    listFiltreDepartementSelected = [];
    filtreEntrepriseSelected;
    constructor(service, parentRouter) {
      this._service = service;
      this._parentRouter = parentRouter;
      this._service.getFiltreEntreprise()
        .then(result => { this.listFiltreEntreprise = result; this.filtreEntrepriseSelected = result[1];});
      this._service.getFiltreDepartement()
        .then(result => {this.listDepartement = result; this.listFiltreDepartementSelected.push(result[1]); this.listFiltreDepartementSelected.push(result[3]);});
      }

    removeFiltreEntreprise(filtre) {
      this.listFiltreDepartementSelected = this.listFiltreDepartementSelected.filter(item => item.label !== filtre.label);
    }
}