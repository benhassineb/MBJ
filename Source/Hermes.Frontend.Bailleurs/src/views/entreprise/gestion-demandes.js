import { Router } from 'aurelia-router';
import { inject } from 'aurelia-framework';

import { ConsulterDemandes } from '../../models/consulter-demandes';
import { ValiderDemande } from '../../models/valider-demande';
import { GererPriorites } from '../../models/gerer-priorites';
import { SampleService } from '../../services/sample-service';

@inject(SampleService, Router)
export class GestionDemandes {

  filtres = [];
  selectedFilter;

  constructor(service, router) {
    this._service = service;
    this._router = router;
    this.filtres.push({ title: 'Veuillez sélectionner un type de filtre', obj: null });
    this.filtres.push({ id: 'consulter-demandes', title: 'Consulter des demandes', obj: new ConsulterDemandes(this._service, this._router) });
    this.filtres.push({ id: 'valider-demandes', title: 'Valider une demande', obj: new ValiderDemande(this._service, this._router) });
    this.filtres.push({ id: 'gerer-priorites', title: 'Gérer mes priorités', obj: new GererPriorites(this._service, this._router)});
  }

  activate(params) {
    if (params && params.filterType) {
      this.selectedFilter = this.filtres.find(f => f.id === params.filterType).obj;
    }
  }

}
