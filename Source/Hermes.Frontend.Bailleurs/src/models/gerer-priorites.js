export class GererPriorites {

  nom;
  prenom;
  dateNaissance;
  matricule;
  fromDate;
  toDate;
  avecJetons;
  enMobilites;
  enAlternance;

  resultats = [];

  constructor(service) {
    this._service = service;
  }

  activate() {
    this._service.getPrioritesAGerer().then(res => this.resultats = res);
  }

}
