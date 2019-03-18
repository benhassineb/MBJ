import { ConsulterDemandes } from '../../models/consulter-demandes';
import { ValiderDemande } from '../../models/valider-demande';
import { GererPriorites } from '../../models/gerer-priorites';
import { inject } from 'aurelia-framework';
import { SampleService } from '../../services/sample-service';

@inject(SampleService)
export class GestionDemandes {

  filtres = [];
  selectedFilter;

  constructor(service) {
    this._service = service;
    this.filtres.push({ title: 'Veuillez sélectionner un type de filtre', obj: null });
    this.filtres.push({ id: 'consulter-demandes', title: 'Consulter des demandes', obj: new ConsulterDemandes(this._service) });
    this.filtres.push({ id: 'valider-demandes', title: 'Valider une demande', obj: new ValiderDemande(this._service) });
    this.filtres.push({ id: 'gerer-priorites', title: 'Gérer mes priorités', obj: new GererPriorites(this._service)});
  }

  activate(params) {
    this.selectedFilter = this.filtres[1];
    if (params && params.filterType) {
      this.selectedFilter = this.filtres.find(f => f.id === params.filterType).obj;
    }
  }

}
