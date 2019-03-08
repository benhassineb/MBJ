import { WebStorageStateStore } from 'oidc-client';
import { AureliaConfiguration } from 'aurelia-configuration';

export default (aurelia) => {
  let configInstance = aurelia.container.get(AureliaConfiguration);
  let oauth2 = configInstance.get('authentication');

  return ({
    unauthorizedRedirectRoute: '/home',
    loginRedirectRoute: '/home',
    logoutRedirectRoute: '/home',
    userManagerSettings: {
      // the number of seconds in advance of access token expiry to raise the access token expiring event.
      accessTokenExpiringNotificationTime: 60,
      // Azure B2C
      authority: `https://${oauth2.tenantName}.b2clogin.com/tfp/${oauth2.tenantName}.onmicrosoft.com/${oauth2.b2cPolicy}/oauth2/v2.0`,
      client_id: oauth2.clientId,
      automaticSilentRenew: true,
      // the interval in milliseconds between checking the user's session.
      checkSessionInterval: 30000,
      filterProtocolClaims: false,
      loadUserInfo: false,
      post_logout_redirect_uri: `${oauth2.redirectHost}/signout-oidc`,
      redirect_uri: `${oauth2.redirectHost}/signin-oidc`,
      response_type: 'id_token token',
      scope: `${oauth2.apiScope} openid`,
      metadata: {
        authorization_endpoint: `https://${oauth2.tenantName}.b2clogin.com/tfp/${oauth2.tenantName}.onmicrosoft.com/${oauth2.b2cPolicy}/oauth2/v2.0/authorize`,
        jwks_uri: `${oauth2.redirectHost}/config/keys.json`,
        end_session_endpoint: `https://${oauth2.tenantName}.b2clogin.com/tfp/${oauth2.tenantName}.onmicrosoft.com/${oauth2.b2cPolicy}/oauth2/v2.0/logout`,
        issuer: `https://${oauth2.tenantName}.b2clogin.com/${oauth2.tenantId}/v2.0/`
      },
      // number of milliseconds to wait for the authorization
      silentRequestTimeout: 10000,
      // server to response to silent renew request
      silent_redirect_uri: `${oauth2.redirectHost}/signin-oidc`,
      userStore: new WebStorageStateStore({
        prefix: 'oidc',
        store: window.localStorage
      }),
      ui_locales: 'fr-FR'
    }
  });

};
