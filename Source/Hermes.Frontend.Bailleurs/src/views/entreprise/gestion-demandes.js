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
  }

  activate() {
    this.filtres.push({ title: 'Veuillez sélectionner un type de filtre', obj: null });
    this.filtres.push({ title: 'Consulter des demandes', obj: new ConsulterDemandes(this._service) });
    this.filtres.push({ title: 'Valider une demande', obj: new ValiderDemande(this._service) });
    this.filtres.push({title: 'Gérer mes priorités', obj: new GererPriorites(this._service)});
  }

}
