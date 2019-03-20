export class DemandeAConsulterItem {

  statuts;
  nom;
  prenom;
  dateNaissance;
  nbrCandidature;
  refusCal;
  refusSalarie;
  jeton;
  dmdPrio;
  matricule;
  dateDemande;

  constructor() {

  }

  static fromObject(...sources) {
    let result = Object.assign(new DemandeAConsulterItem(), ...sources);
    return result;
  }

}
