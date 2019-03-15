export class BailleurRouter {

  configureRouter(config, router) {
    config.map([
      { route: ['', 'accueil'], name: 'accueil', moduleId: './accueil-bailleur', nav: true, title: 'Accueil' },
      { route: 'bailleur', name: 'bailleur', moduleId: 'bailleur-router', nav: true, title: 'bailleur Router' }
    ]);

    this.router = router;
  }

}
