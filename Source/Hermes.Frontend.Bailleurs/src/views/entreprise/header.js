import { inject } from 'aurelia-framework';
import { SampleService } from 'services/sample-service';

@inject(SampleService)
export class header {
    listFiltreEntreprise;
    listFiltreEntrepriseSelected = [];
    
    constructor(service) {
        this._service = service;
        this._service.getFiltreEntreprise()
      .then(result => {this.listFiltreEntreprise = result; this.listFiltreEntrepriseSelected.push(this.listFiltreEntreprise[1]);  });
    }

    removeFiltreEntreprise(filtre) {
        let listFiltreEntrepriseSelected = listFiltreEntrepriseSelected.filter(item => item.code !== filtre.code);
      }
}