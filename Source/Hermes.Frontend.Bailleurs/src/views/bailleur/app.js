export class App {

  configureRouter(config, router) {
    config.map([
      { route: ['', 'accueil'], name: 'accueil', moduleId: './accueil', nav: true, title: 'Accueil', breadcrumb: true },
      { route: ['connexion'],   name: 'connexion', moduleId: './connexion', nav: true, title: 'Connexion', breadcrumb: true },
      { route: ['deconnexion'], name: 'deconnexion', moduleId: './deconnexion', nav: true, title: 'Deconnexion', breadcrumb: true },
      { route: ['inscription'], name: 'inscription', moduleId: './inscription', nav: true, title: 'Inscription', breadcrumb: true },
      { route: ['inscription-to-validate'], name: 'inscription-to-validate', moduleId: './inscription-to-validate', nav: true, title: 'Inscription a valider', breadcrumb: true },
      { route: ['info-connexion'], name: 'info-connexion', moduleId: './info-connexion', nav: true, title: 'Info Connexion', breadcrumb: true },
      { route: ['suivi-placement'], name: 'suivi-placement', moduleId: './suivi-placement', nav: true, title: 'Suivi Placement', breadcrumb: true },
      { route: ['depot-logements'], name: 'depot-logements', moduleId: './depot-logements', nav: true, title: 'Depot Logements', breadcrumb: true },
      { route: ['logement'],    name: 'logement', moduleId: './logement', nav: true, title: 'Logement', breadcrumb: true },
      { route: ['candidat'],    name: 'candidat', moduleId: './candidat', nav: true, title: 'Candidat', breadcrumb: true },
      { route: ['list-candidat'], name: 'list-candidat', moduleId: './list-candidat', nav: true, title: 'List Candidat', breadcrumb: true },
      { route: ['convention-flux'], name: 'convention-flux', moduleId: './convention-flux', nav: true, title: 'Convention Flux', breadcrumb: true },
      { route: ['conventions-financement'], name: 'conventions-financement', moduleId: './conventions-financement', nav: true, title: 'Conventions Financement', breadcrumb: true },
      { route: ['echanges'],    name: 'echanges', moduleId: './echanges', nav: true, title: 'Echanges', breadcrumb: true },
      { route: ['mon-compte'],  name: 'mon-compte', moduleId: './mon-compte', nav: true, title: 'Mon Compte', breadcrumb: true }
    ]);

    this.router = router;
  }

}
