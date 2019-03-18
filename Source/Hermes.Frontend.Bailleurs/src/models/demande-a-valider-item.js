export class DemandeAValiderItem {

  nom;
  prenom;
  dateNaissance;
  matricule;
  dateDemande;

  constructor() {

  }

  static fromObject(...sources) {
    let result = Object.assign(new DemandeAValiderItem(), ...sources);
    return result;
  }

}
