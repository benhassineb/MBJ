export class App {

  configureRouter(config, router) {
    config.map([
      { route: ['', 'accueil'], name: 'accueil', moduleId: './accueil', nav: true, title: 'Accueil' }
    ]);

    this.router = router;
  }

}
