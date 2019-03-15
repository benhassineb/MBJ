import { ConsulterDemandes } from '../../models/consulter-demandes';
import { ValiderDemande } from '../../models/valider-demande';
import { GererPriorites } from '../../models/gerer-priorites';

export class GestionDemandes {

  filtres = [];
  selectedFilter;

  constructor() {
    this.filtres.push({ title: 'Veuillez sélectionner un type de filtre', obj: null });
    this.filtres.push({ title: 'Consulter des demandes', obj: new ConsulterDemandes() });
    this.filtres.push({ title: 'Valider une demande', obj: new ValiderDemande() });
    this.filtres.push({title: 'Gérer mes priorités', obj: new GererPriorites()});
  }

}
