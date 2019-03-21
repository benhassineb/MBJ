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
    if (result.dmdPrio === undefined) result.dmdPrio = null;
    return result;
  }

}
