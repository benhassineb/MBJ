export class DemandeAValiderItem {

  nom;
  prenom;
  dateNaissance;
  matricule;
  dateDemande;
  dmdValide; //true, false ou null
  dmdValideOui;
  dmdValideNon;
  prioritaire;

  constructor() {

  }

  static fromObject(...sources) {
    let result = Object.assign(new DemandeAValiderItem(), ...sources);
    if (result.dmdValide === undefined) {
      result.dmdValide = null;
    }
    else {
      result.dmdValideOui = (result.dmdValide);
      result.dmdValideNon = (!result.dmdValide);
    }
    return result;
  }

  handleClick(val) {
    if (this.dmdValide === val) {
      this.dmdValide = null;
      this.dmdValideOui = this.dmdValide;
      this.dmdValideNon = this.dmdValide;
    }
    else {
      this.dmdValide = val; // toggle clicked true/false
      this.dmdValideOui = (val);
      this.dmdValideNon = (!this.dmdValideOui);
    }
    return true; // only needed if you want to cancel preventDefault()
  }

}
