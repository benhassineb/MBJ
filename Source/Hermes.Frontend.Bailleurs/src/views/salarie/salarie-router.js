export class SalarieRouter {

  configureRouter(config, router) {
    config.map([
      { route: ['', 'acceuil'], name: 'acceuil', moduleId: './acceuil-salarie', nav: true, title: 'Acceuil' },
      { route: 'salarie', name: 'salarie', moduleId: 'salarie-router', nav: true, title: 'Salarie Router' }
    ]);

    this.router = router;
  }

}
