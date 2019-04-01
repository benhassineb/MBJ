import { inject } from 'aurelia-framework';
import { SampleService } from 'services/sample-service';
import { Router } from 'aurelia-router';
import { Auth } from './auth';

@inject(SampleService, Router, Auth)
export class Header {

  test;
  loadingText = 'Chargement...';
  noResultsText = 'Aucun résultat...';
  listDepartement;
  listFiltreEntreprise;
  listFiltreDepartementSelected = [];
  filtreEntrepriseSelected;
  filtreFilialeSelected;
  departementSelected;

  constructor(service, parentRouter, auth) {
    this.auth = auth;
    this._service = service;
    this._parentRouter = parentRouter;
    this._service.getFiltreEntreprise()
      .then(result => { this.listFiltreEntreprise = result; });
    this._service.getFiltreDepartement()
      .then(result => {
        this.listDepartement = result;
        this.listFiltreDepartementSelected.push(result[1]);
        this.listFiltreDepartementSelected.push(result[3]);
      });
    this._service.getFiltreEntreprise()
    .then(result => { this.filtreEntrepriseSelected = result[0]; });
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
    this._clearSelectdDepartement = setTimeout(() => {
      _this.clearDepartement();
    }, 10);
  }

  clearDepartement() {
    this.departementSelected = null;
    this.filter = null;
    clearTimeout(this._clearSelectdDepartement);
  }
  
}
