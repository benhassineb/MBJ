export class Master {

  configureRouter(config, router) {
    config.map([
      { route: ['', 'accueil'], name: 'accueil', moduleId: './accueil-entreprise', nav: true, title: 'Accueil', breadcrumb: true },
      { route: 'details-demande-sans-cerfa',  name: 'details-demande-sans-cerfa', moduleId: './details-demande-sans-cerfa', nav: true, title: 'Détails Demande (sans cerfa)', breadcrumb: true  },
      { route: 'details-demande-avec-cerfa',  name: 'details-demande-avec-cerfa', moduleId: './details-demande-avec-cerfa', nav: true, title: 'Détails Demande (avec cerfa)', breadcrumb: true },
      { route: 'gestion-demandes/:filterType?', name: 'gestion-demandes', moduleId: './gestion-demandes', nav: false, title: 'Gestion Demandes', breadcrumb: true },
      { route: 'offres-privilege-consulte', name: 'offres-privilege-consulte', moduleId: './offres-privilege-consulte', nav: true, title: 'Offres Privilège (consultation)', breadcrumb: true },
      { route: 'offres-privilege-edite', name: 'offres-privilege-edite', moduleId: './offres-privilege-edite', nav: true, title: 'Offres Privilège (édition)', breadcrumb: true }
    ]);

    this.router = router;
  }

}
