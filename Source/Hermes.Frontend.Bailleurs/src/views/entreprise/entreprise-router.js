export class EntrepriseRouter {

  configureRouter(config, router) {
    config.map([
      { route: ['', 'accueil'], name: 'accueil', moduleId: './accueil-entreprise', nav: true, title: 'Accueil' },
      { route: 'Entreprise', name: 'Entreprise', moduleId: 'entreprise-router', nav: true, title: 'entreprise Router' }
    ]);

    this.router = router;
  }

}
