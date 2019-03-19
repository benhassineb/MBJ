import { inject } from 'aurelia-framework';
import { SampleService } from 'services/sample-service';
import {customElement, bindable, bindingMode } from 'aurelia-framework';
import {Router} from 'aurelia-router';
import { OpenIdConnect } from 'aurelia-open-id-connect';

@inject(SampleService, Router, OpenIdConnect)
@customElement('header-entreprise')
export class headerentreprise {

    listDepartement;
    listFiltreEntreprise;
    listFiltreDepartementSelected = [];
    filtreEntrepriseSelected;
    _openIdConnect;
    user;
    constructor(service, parentRouter, openIdConnect) {
      this._service = service;
      this._parentRouter = parentRouter;
      this._openIdConnect = openIdConnect
      this._service.getFiltreEntreprise()
        .then(result => { this.listFiltreEntreprise = result; this.filtreEntrepriseSelected = result[1];});
      this._service.getFiltreDepartement()
        .then(result => {this.listDepartement = result; this.listFiltreDepartementSelected.push(result[1]); this.listFiltreDepartementSelected.push(result[3]);});
      }

    removeFiltreEntreprise(filtre) {
      this.listFiltreDepartementSelected = this.listFiltreDepartementSelected.filter(item => item.label !== filtre.label);
    }

    activate(){
      this._openIdConnect.getUser().then(user => {
        this.user = user;
      });
    }
  
    get isLoggedIn() {
      return this.user !== null && this.user !== undefined;
    }
}