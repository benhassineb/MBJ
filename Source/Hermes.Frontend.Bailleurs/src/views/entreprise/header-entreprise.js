import { inject } from 'aurelia-framework';
import { SampleService } from 'services/sample-service';
import {customElement, bindable, bindingMode } from 'aurelia-framework';
import {Router} from 'aurelia-router';
import { OpenIdConnect } from 'aurelia-open-id-connect';

@inject(SampleService, Router, OpenIdConnect)
export class HeaderEntreprise {

    test;
    loadingText = 'Chargement...';
    noResultsText = 'Aucun résultat...';
    listDepartement;
    listFiltreEntreprise;
    listFiltreDepartementSelected = [];
    filtreEntrepriseSelected;
    filtreFilialeSelected;
    _openIdConnect;
    departementSelected;

    constructor(service, parentRouter, openIdConnect) {
      this._service = service;
      this._parentRouter = parentRouter;
      this._openIdConnect = openIdConnect
      this._service.getFiltreEntreprise()
        .then(result => { this.listFiltreEntreprise = result});
      this._service.getFiltreDepartement()
        .then(result => {this.listDepartement = result; this.listFiltreDepartementSelected.push(result[1]); this.listFiltreDepartementSelected.push(result[3]);});
      }

    removeFiltreEntreprise(filtre) {
      this.listFiltreDepartementSelected = this.listFiltreDepartementSelected.filter(item => item.label !== filtre.label);
    }

    getListeDepartement(filter) {
      return this._service.getFiltreDepartement(filter)
        .then(result => this.listDepartement = result.filter(item => item.label.toLowerCase().includes(filter.toLowerCase())))
        .then(result => result.filter(item => !this.listFiltreDepartementSelected.some(dep => dep.code === item.code)));
    }

    _clearSelectdDepartement;
    onSelectDepartement(item) {
      if (item) {
      this.listFiltreDepartementSelected.push(item);
      }
      let _this = this;
      this._clearSelectdDepartement = setTimeout(function(){ 
        _this.clearDepartement(); 
      }, 10);
    }

    clearDepartement() {
      this.departementSelected = null;
      this.filter = null;
      clearTimeout(this._clearSelectdDepartement);
    }
}
