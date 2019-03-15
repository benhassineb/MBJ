export class EntrepriseRouter {

  configureRouter(config, router) {
    config.map([
      { route: ['', 'accueil'], name: 'accueil', moduleId: './accueil-entreprise', nav: true, title: 'Accueil' },
      { route: 'pE2_1', name: 'pE2_1', moduleId: './pE2_1', nav: true, title: 'pE2_1' },

      { route: 'Entreprise', name: 'Entreprise', moduleId: 'entreprise-router', nav: true, title: 'entreprise Router' }
    ]);

    this.router = router;
  }

}
