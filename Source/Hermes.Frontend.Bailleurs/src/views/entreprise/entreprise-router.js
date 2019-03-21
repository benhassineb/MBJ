export class EntrepriseRouter {

  configureRouter(config, router) {
    config.map([
      { route: ['', 'accueil'], name: 'accueil', moduleId: './accueil-entreprise', nav: true, title: 'Accueil', breadcrumb: true },
      { route: 'details-demande-sans-cerfa',  name: 'details-demande-sans-cerfa', moduleId: './details-demande-sans-cerfa', nav: true, title: 'Details demande sans cerfa', breadcrumb: true  },
      { route: 'details-demande-avec-cerfa',  name: 'details-demande-avec-cerfa', moduleId: './details-demande-avec-cerfa', nav: true, title: 'Details demande avec cerfa', breadcrumb: true },
      { route: 'Entreprise', name: 'Entreprise', moduleId: 'entreprise-router', nav: true, title: 'entreprise Router', breadcrumb: false },
      { route: 'gestion-demandes/:filterType?', name: 'gestion-demandes', moduleId: './gestion-demandes', nav: true, title: 'Gestion des demandes', href: '#gestion-demandes', breadcrumb: true },
      { route: 'offres-privilege-consulte', name: 'offres-privilege-consulte', moduleId: './offres-privilege-consulte', nav: true, title: 'Offres Privilège', breadcrumb: true },
      { route: 'offres-privilege-edite', name: 'offres-privilege-edite', moduleId: './offres-privilege-edite', nav: true, title: 'Offres Privilège', breadcrumb: true }

    ]);

    this.router = router;
  }

}
