import { ConsulterDemandes } from '../../models/consulter-demandes';
import { ValiderDemande } from '../../models/valider-demande';

export class PageE2_1 {

  filtres = [];
  selectedFilter;

  constructor() {
    this.filtres.push({ title: 'Veuillez sélectionner un type de filtre', obj: null });
    this.filtres.push({ title: 'Consulter des demandes', obj: new ConsulterDemandes() });
    this.filtres.push({ title: 'Valider une demande', obj: new ValiderDemande() });
  }

}
