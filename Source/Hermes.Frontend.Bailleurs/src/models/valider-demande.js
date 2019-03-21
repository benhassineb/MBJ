export class ValiderDemande {
  fromDate;
  toDate;
  nom;
  prenom;
  dateNaissance;
  matricule;

  resultats = [];

  currentPage = 1;
  pageSize = 5;

  constructor(service) {
    this._service = service;
  }

  activate() {
    this._service.getDemandesAValider().then(res => this.resultats = res);
  }

  search() {
    this._service.getDemandesAValider().then(res => {
      if (this.nom) {
        res = res.filter(m=>m.nom.toLowerCase().indexOf(this.nom.toLowerCase()) > -1);
      }
      if (this.prenom) {
        res = res.filter(m=>m.prenom.toLowerCase().indexOf(this.prenom.toLowerCase()) > -1);
      }
      if (this.dateNaissance) {
        res = res.filter(m=>m.dateNaissance === this.dateNaissance);
      }
      if (this.matricule) {
        res = res.filter(m=>m.matricule.toLowerCase().indexOf(this.matricule.toLowerCase()) > -1);
      }
      if (this.fromDate) {
        let fromDate = new Date(this.fromDate);
        res = res.filter(m=> new Date(m.dateDemande) >= fromDate);
      }
      if (this.toDate) {
        let toDate = new Date(this.toDate);
        res = res.filter(m=> new Date(m.dateDemande) <= toDate);
      }
      this.resultats = res;
    });
  }

}
