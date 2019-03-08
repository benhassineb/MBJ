import environment from './environment';
import configureAuth from 'config/auth-config-v2';

export function configure(aurelia) {
  aurelia.use
    .standardConfiguration()
    .feature('resources')
    .plugin('aurelia-configuration')
    .plugin('aurelia-open-id-connect', () => configureAuth(aurelia));

  if (environment.debug) {
    aurelia.use.developmentLogging();
  }

  if (environment.testing) {
    aurelia.use.plugin('aurelia-testing');
  }

  aurelia.start().then(() => aurelia.setRoot());
}
