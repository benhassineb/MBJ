export class App {

  configureRouter(config, router) {
    config.map([
      { route: ['', 'accueil'], name: 'accueil', moduleId: './accueil', nav: true, title: 'Accueil' },
      { route: 'connexion', name: 'connexion', moduleId: './connexion', nav: true, title: 'Connexion' },
      { route: 'deconnexion', name: 'deconnexion', moduleId: './deconnexion', nav: true, title: 'Déconnexion' },
      { route: 'profil', name: 'profil', moduleId: './profil', nav: true, title: 'Profil' },
      { route: 'recherche', name: 'recherche', moduleId: './recherche', nav: true, title: 'Recherche' },
      { route: 'inscription', name: 'inscription', moduleId: './inscription', nav: true, title: 'Inscription' },
      { route: 'logement', name: 'logement', moduleId: './logement', nav: true, title: 'Logement' },
      { route: 'inscription-nud', name: 'inscription-nud', moduleId: './inscription-nud', nav: true, title: 'Inscription-nud' },
      { route: 'saisie-demande', name: 'saisie-demande', moduleId: './saisie-demande/saisie-demande', nav: true, title: 'Saisie Demande' },
      { route: 'candidater', name: 'candidater', moduleId: './candidater', nav: true, title: 'Candidater' }
    ]);
    this.router = router;
  }

}
