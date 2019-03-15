export class EntrepriseRouter {

  configureRouter(config, router) {
    config.map([
      { route: ['', 'accueil'], name: 'accueil', moduleId: './accueil-entreprise', nav: true, title: 'Accueil' },
      { route: 'gestion-demandes', name: 'gestion-demandes', moduleId: './gestion-demandes', nav: true, title: 'Gestion des demandes' },

      { route: 'Entreprise', name: 'Entreprise', moduleId: 'entreprise-router', nav: true, title: 'entreprise Router' }
    ]);

    this.router = router;
  }

}
