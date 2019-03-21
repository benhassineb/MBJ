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

  constructor(service, router) {
    this._service = service;
    this._router = router;
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

  navigateToDetail(pIndex) {
    let id = (this.currentPage - 1) * this.pageSize + pIndex + 1;
    this._router.navigateToRoute('details-demande-avec-cerfa', { id: id });
  }

}
