import { inject } from 'aurelia-framework';
import { Auth } from '../auth';
import { Router } from 'aurelia-router';
import { SaisieEtatCivil } from './saisie-etat-civil';
import { SaisieDomicileActuel } from './saisie-domicile-actuel';
import { SaisieSituationFamiliale } from './saisie-situation-familiale';
import { SaisieSituationPro } from './saisie-situation-pro';
import { SaisieSituationFinanciere } from './saisie-situation-financiere';
import { SaisieLogement } from './saisie-logement';
import { SaisieRecap } from './saisie-recap';
import { SaisieComplete } from './saisie-complete';

@inject(Auth, Router, SaisieEtatCivil, SaisieDomicileActuel, SaisieSituationFamiliale,
  SaisieSituationPro, SaisieSituationFinanciere, SaisieLogement, SaisieRecap, SaisieComplete)
export class SaisieDemande {
  etapes = [];
  selectedStep;
  constructor(auth, router, etatCivil, domicileActuel, situationFamiliale, situationPro,
    situationFinanciere, logement, recap, completeSaisie) {
    this.auth = auth;
    this.router = router;
    this.etapes.push(etatCivil);
    this.etapes.push(domicileActuel);
    this.etapes.push(situationFamiliale);
    this.etapes.push(situationPro);
    this.etapes.push(situationFinanciere);
    this.etapes.push(logement);
    this.etapes.push(recap);
    this.etapes.push(completeSaisie);
    this.selectedStep = etatCivil;
  }
  goToStep(step) {
    this.selectedStep = step;
  }
  next() {
    const indexActuel = this.etapes.indexOf(this.selectedStep);
    if (indexActuel < this.etapes.length) this.selectedStep = this.etapes[indexActuel + 1];
  }
  previous() {
    const indexActuel = this.etapes.indexOf(this.selectedStep);
    if (indexActuel > this.etapes.length) this.selectedStep = this.etapes[indexActuel - 1];
  }
}
