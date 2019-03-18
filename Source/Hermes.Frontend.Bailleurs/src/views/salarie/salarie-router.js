import { inject } from 'aurelia-framework';
import { Auth } from './auth';

@inject(Auth)
export class SalarieRouter {

  constructor(auth) {
    this.auth = auth;
  }

  configureRouter(config, router) {
    config.title = 'Hermes';
    config.map([
      { route: ['', 'accueil'], name: 'accueil', moduleId: './viewmodels/accueil', nav: false, title: 'accueil' },
      { route: 'connexion', name: 'connexion', moduleId: './viewmodels/connexion', nav: !this.auth.isLoggedIn, title: 'connexion' },
      { route: 'profil', name: 'profil', moduleId: './viewmodels/profil', nav: this.auth.isLoggedIn, title: 'profil' },
      { route: 'rechercher', name: 'rechercher', moduleId: './viewmodels/rechercher', nav: false, title: 'rechercher' },
      { route: 'inscription', name: 'inscription', moduleId: './viewmodels/inscription', nav: false, title: 'inscription' },
      { route: 'logement', name: 'logement', moduleId: './viewmodels/logement', nav: false, title: 'logement' },
      { route: 'deconnexion', name: 'deconnexion', moduleId: './viewmodels/deconnexion', nav: this.auth.isLoggedIn, title: 'deconnexion' },
      { route: 'inscription-nud', name: 'inscription-nud', moduleId: './viewmodels/inscription-nud', nav: false, title: 'inscription-nud' },
      { route: 'saisie-demande', name: 'saisie-demande', moduleId: './viewmodels/saisie-demande', nav: false, title: 'saisie-demande' },
      { route: 'salarie', name: 'salarie', moduleId: 'salarie-router', nav: true, title: 'Salarie Router' }

    ]);
    this.router = router;
  }

}
