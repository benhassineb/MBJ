import {headerentreprise} from './header-entreprise';

export class EntrepriseRouter {

  configureRouter(config, router) {
    config.map([
      { route: ['', 'accueil'], name: 'accueil', moduleId: './accueil-entreprise', nav: true, title: 'Accueil', breadcrumb: true },
      { route: 'details-demande-sans-CERFA',  name: 'details-demande-sans-CERFA', moduleId: './details-demande-sans-CERFA', nav: true, title: 'Details demande sans CERFA', breadcrumb: true  },
      { route: 'details-demande-avec-CERFA',  name: 'details-demande-avec-CERFA', moduleId: './details-demande-avec-CERFA', nav: true, title: 'Details demande avec CERFA', breadcrumb: true },
      { route: 'Entreprise', name: 'Entreprise', moduleId: 'entreprise-router', nav: true, title: 'entreprise Router', breadcrumb: false },
      { route: 'gestion-demandes/:filterType?', name: 'gestion-demandes', moduleId: './gestion-demandes', nav: true, title: 'Gestion des demandes', href: '#gestion-demandes', breadcrumb: true },
      { route: 'offres-privilege-consulte', name: 'offres-privilege-consulte', moduleId: './offres-privilege-consulte', nav: true, title: 'Offres Privilège', breadcrumb: true },
      { route: 'offres-privilege-edite', name: 'offres-privilege-edite', moduleId: './offres-privilege-edite', nav: true, title: 'Offres Privilège', breadcrumb: true }

    ]);

    this.router = router;
  }

}
