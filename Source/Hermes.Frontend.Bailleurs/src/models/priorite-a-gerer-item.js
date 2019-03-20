export class PrioriteAGererItem {

  nom;
  prenom;
  dateNaissance;
  matricule;
  dateDemande;
  situation;
  prioritaire;

  constructor() {

  }

  static fromObject(...sources) {
    let result = Object.assign(new PrioriteAGererItem(), ...sources);
    if (result.prioritaire === undefined) result.prioritaire = false;
    return result;
  }

}
