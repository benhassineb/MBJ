export class ConsulterDemandes {
  statuts = ['Incomplètes', 'Satisfaites', 'Annulées', 'En cours'];
  nbrCandidature;
  nbreRefusEnCal;
  nbreRefusParClientEnCal;
  dmdPrioritaire;
  salarieCoache;
  fromDate;
  toDate;
  nom;
  prenom;
  dateNaissance;
  matricule;

  resultats = [];

  constructor(service) {
    this._service = service;
  }

  activate() {
    this._service.getDemandesAConsulter().then(res => this.resultats = res);
  }

}
