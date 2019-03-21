import { inject } from 'aurelia-framework';
import { APPLICATIONTITLE, SITEMAP } from 'config/app-config';
import { ConfiguredHttpClient } from 'core/configured-http-client';
import { OpenIdConnect } from 'aurelia-open-id-connect';

@inject(OpenIdConnect, ConfiguredHttpClient)
export class App {

  user;
  /**
   * Crée un instance de l'application aurelia.
   * @param {OpenIdConnect} openIdConnect - la couche de connexion OpenID Connect
   * @param {ConfiguredHttpClient} configuredhttpclient - le service de récupération des données
   */
  constructor(openIdConnect, configuredhttpclient) {
    this._openIdConnect = openIdConnect;
    this.httpClient = configuredhttpclient.httpClient;
    this._openIdConnect.logger.setLogLevel(3);
  }

  /**
   * Configure le routeur de l'application le service de récupération des données.
   * @param {RouterConfiguration} configuration - la configuration du routeur
   * @param {Router} router - le routeur aurelia
   */
  configureRouter(configuration, router) {
    configuration.title = APPLICATIONTITLE;
    configuration.options.pushState = true;
    configuration.options.root = '/';
    configuration.map([
      { route: 'bailleur', name: 'bailleur', moduleId: 'views/bailleur/master', nav: true, title: 'Site Bailleurs', breadcrumb: true },
      { route: 'entreprise', name: 'entreprise', moduleId: 'views/entreprise/master', nav: true, title: 'Site Entreprises', breadcrumb: true },
      { route: ['', 'salarie'], name: 'salarie', moduleId: 'views/salarie/master', nav: true, title: 'Site Salariés', breadcrumb: true }

    ]);
    // configuration.fallbackRoute(SITEMAP.home.title);
    this._openIdConnect.configure(configuration);
    this.router = router;
    //this.registerNavigationSteps(configuration);
  }

  activate() {
    this._openIdConnect.getUser().then(user => {
      this.user = user;
    });
  }

  get isLoggedIn() {
    return this.user !== null && this.user !== undefined;
  }
}
