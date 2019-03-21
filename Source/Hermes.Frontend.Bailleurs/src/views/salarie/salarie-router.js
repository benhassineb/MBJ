import { inject } from 'aurelia-framework';
import { Auth } from './auth';
import { HeaderSalarie } from './viewmodels/header-salarie';

@inject(Auth, HeaderSalarie)
export class SalarieRouter {

  constructor(auth, header) {
    this.auth = auth;
    this.header = header;
  }

  configureRouter(config, router) {
    config.title = 'Hermes';
    config.map([
      { route: ['', 'accueil'], name: 'accueil', moduleId: './viewmodels/accueil', nav: true, title: 'Accueil' },
      { route: 'connexion', name: 'connexion', moduleId: './viewmodels/connexion', nav: true, title: 'Connexion' },
      { route: 'profil', name: 'profil', moduleId: './viewmodels/profil', nav: true, title: 'Profil' },
      { route: 'rechercher', name: 'rechercher', moduleId: './viewmodels/rechercher', nav: false, title: 'rechercher' },
      { route: 'inscription', name: 'inscription', moduleId: './viewmodels/inscription', nav: false, title: 'inscription' },
      { route: 'logement', name: 'logement', moduleId: './viewmodels/logement', nav: false, title: 'logement' },
      { route: 'deconnexion', name: 'deconnexion', moduleId: './viewmodels/deconnexion', nav: true, title: 'Deconnexion' },
      { route: 'inscription-nud', name: 'inscription-nud', moduleId: './viewmodels/inscription-nud', nav: false, title: 'inscription-nud' },
      { route: 'saisie-demande', name: 'saisie-demande', moduleId: './viewmodels/saisie-demande', nav: false, title: 'saisie-demande' },
      { route: 'complete-saisie', name: 'complete-saisie', moduleId: './viewmodels/complete-saisie', nav: false, title: 'complete-saisie' },
      { route: 'candidater', name: 'candidater', moduleId: './viewmodels/candidater', nav: false, title: 'candidater' },
      { route: 'salarie', name: 'salarie', moduleId: 'salarie-router', nav: false, title: 'Salarie Router' }

    ]);
    this.router = router;
  }

}
