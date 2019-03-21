export class Master {

  configureRouter(config, router) {
    config.map([
      { route: ['', 'accueil'], name: 'accueil', moduleId: './accueil-bailleur', nav: true, title: 'Accueil' }
    ]);

    this.router = router;
  }

}
