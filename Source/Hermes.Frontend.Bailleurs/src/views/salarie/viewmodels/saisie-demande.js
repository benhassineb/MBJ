import { inject } from 'aurelia-framework';
import { Auth } from '../auth';
import { Router } from 'aurelia-router';
import { SaisieDemandeur } from './saisie-demandeur';
import { SaisieSituation } from './saisie-situation';
import { SaisieRessource } from './saisie-ressource';
import { SaisieLogement } from './saisie-logement';
import { SaisieRecap } from './saisie-recap';

@inject(Auth, Router, SaisieDemandeur, SaisieSituation, SaisieRessource, SaisieLogement, SaisieRecap)
export class SaisieDemande {
  etapes = [];
  selectedStep;
  constructor(auth, router, demandeur, situation, ressource, logement, recap) {
    this.auth       = auth;
    this.router     = router;
    this.etapes.push(demandeur);
    this.etapes.push(situation);
    this.etapes.push(ressource);
    this.etapes.push(logement );
    this.etapes.push(recap );
    this.selectedStep = demandeur;
  }
  goToStep(step) {
    this.selectedStep = step;
  }
}
