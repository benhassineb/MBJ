export class DemandeAConsulterItem {

  nom;
  prenom;
  dateNaissance;
  nbrCandidature;
  refusCal;
  refusSalaire;
  jeton;
  dmdPrio;
  matricule;

  constructor() {

  }

  static fromObject(...sources) {
    let result = Object.assign(new DemandeAConsulterItem(), ...sources);
    return result;
  }

}
