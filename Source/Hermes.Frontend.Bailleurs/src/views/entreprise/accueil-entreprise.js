import { inject } from 'aurelia-framework';
import { SampleService } from 'services/sample-service';

@inject(SampleService)
export class AcceuilEntreprise {

  allEntreprise = [];
  entreprise;
  listFiltreEntreprise;
  listFiltreEntrepriseSelected = [];
  constructor(service) {
    this._service = service;
    this._service.getLogements()
      .then(result => this.logements = result);
    this._service.getCommunes()
      .then(result => this.communes = result);
    this._service.getReseauFerre()
      .then(result => this.reseauferre = result);
    this._service.getFiltreEntreprise()
      .then(result => {this.listFiltreEntreprise = result; this.listFiltreEntrepriseSelected.push(this.listFiltreEntreprise[1]);  });

    
  }

  effacerCacheAutorisations() {
    return this._service.effacerCacheAutorisations('test')
      .then(result => this.result = result);
  }

  removeFiltreEntreprise(filtre) {
    let listFiltreEntrepriseSelected = listFiltreEntrepriseSelected.filter(item => item.code !== filtre.code);
  }
  
}

