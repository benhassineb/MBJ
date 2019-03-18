export class ValiderDemande {
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
    this._service.getDemandesAValider().then(res => this.resultats = res);
  }

}
