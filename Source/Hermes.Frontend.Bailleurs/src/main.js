import environment from './environment';
import configureAuth from 'config/auth-config-v2';
import Backend from 'i18next-xhr-backend';
import { TCustomAttribute } from 'aurelia-i18n';

export function configure(aurelia) {
  aurelia.use
    .standardConfiguration()
    .feature('resources')
    .plugin('aurelia-configuration')
    .plugin('aurelia-crumbs')
    .plugin('aurelia-open-id-connect', () => configureAuth(aurelia))
    .plugin('aurelia-i18n', (instance) => {
      let aliases = ['t', 'i18n'];
      TCustomAttribute.configureAliases(aliases);
      instance.i18next.use(Backend);
      return instance.setup({
        fallbackLng: 'fr',
        whitelist: ['fr'],
        preload: ['fr'],
        ns: 'global',
        defaultNS: 'global',
        fallbackNS: false,
        attributes: aliases,
        lng: 'fr',
        debug: environment.debug,
        backend: {
          loadPath: './locales/{{lng}}/{{ns}}.json'
        }
      });
    });

  if (environment.debug) {
    aurelia.use.developmentLogging();
  }

  if (environment.testing) {
    aurelia.use.plugin('aurelia-testing');
  }

  aurelia.start().then(() => aurelia.setRoot());
}
