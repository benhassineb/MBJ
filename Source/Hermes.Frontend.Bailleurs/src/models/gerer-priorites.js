export class GererPriorites {

  nom;
  prenom;
  dateNaissance;
  matricule;
  fromDate;
  toDate;
  enMobilites = true;
  enAlternance = true;

  avecJetons = null;
  avecJetonsOui;
  avecJetonsNon;

  resultats = [];

  currentPage = 1;
  pageSize = 5;

  constructor(service) {
    this._service = service;
  }

  activate() {
    this._service.getPrioritesAGerer().then(res => this.resultats = res);
  }

  search() {
    this._service.getPrioritesAGerer().then(res => {
      if (this.nbrCandidature) {
        res = res.filter(m => m.nbrCandidature === this.nbrCandidature);
      }
      if (this.nbreRefusEnCal) {
        res = res.filter(m => m.nbreRefusEnCal === this.nbreRefusEnCal);
      }
      if (this.nbreRefusParClientEnCal) {
        res = res.filter(m => m.nbreRefusParClientEnCal === this.nbreRefusParClientEnCal);
      }
      if (this.nom) {
        res = res.filter(m => m.nom.toLowerCase().indexOf(this.nom.toLowerCase()) > -1);
      }
      if (this.prenom) {
        res = res.filter(m => m.prenom.toLowerCase().indexOf(this.prenom.toLowerCase()) > -1);
      }
      if (this.dateNaissance) {
        res = res.filter(m => m.dateNaissance === this.dateNaissance);
      }
      if (this.matricule) {
        res = res.filter(m => m.matricule.toLowerCase().indexOf(this.matricule.toLowerCase()) > -1);
      }
      if (!this.enAlternance) {
        res = res.filter(m => m.situation !== 'Alternance');
      }
      if (!this.enMobilites) {
        res = res.filter(m => m.situation !== 'Mobilité');
      }
      if (this.avecJetons !== null) {
        res = res.filter(m => m.prioritaire === this.avecJetons);
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

  handleClick(val) {
    if (this.avecJetons === val) {
      this.avecJetons = null;
      this.avecJetonsOui = this.avecJetons;
      this.avecJetonsNon = this.avecJetons;
    }
    else {
      this.avecJetons = val; // toggle clicked true/false
      this.avecJetonsOui = (val);
      this.avecJetonsNon = (!this.avecJetonsOui);
    }
    return true; // only needed if you want to cancel preventDefault()
  }

}
