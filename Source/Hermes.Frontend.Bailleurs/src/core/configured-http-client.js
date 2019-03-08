import { inject, computedFrom } from 'aurelia-framework';
import { HttpClient } from 'aurelia-fetch-client';
import { Router } from 'aurelia-router';
import { OpenIdConnect } from 'aurelia-open-id-connect';
import { AureliaConfiguration } from 'aurelia-configuration';
import { delay } from 'core/common';

@inject(HttpClient, OpenIdConnect, Router, AureliaConfiguration)
export class ConfiguredHttpClient {

  constructor(httpclient, openidconnect, router, aureliaConfiguration) {
    this._openIdConnect = openidconnect;
    this._router = router;
    this.api = aureliaConfiguration.get('api.sample');
    httpclient.configure(config => {
      config
        .withBaseUrl(this.api)
        .withDefaults({
          headers: {
            'Access-Control-Allow-Credentials': 'true',
            'Accept': 'application/json'
          },
          'credentials': 'include',
          'mode': 'cors'
        })
        .withInterceptor({
          request(request) {
            delay(() => httpclient.isRequesting = true);
            return request;
          },
          requestError(error) {
            delay(() => httpclient.isRequesting = false);
            return error;
          },
          response(response) {
            delay(() => httpclient.isRequesting = false);
            return response;
          },
          responseError(error) {
            delay(() => httpclient.isRequesting = false);
            return error;
          }
        })
        .rejectErrorResponses();
    });
    openidconnect.observeUser(user => {
      this.currentUser = user;
      if (this.isLoggedIn) {
        let _headers = {
          'Authorization': `Bearer ${user.access_token}`
        };
        httpclient.configure(config => config.withDefaults({
          headers: _headers
        }));
      }
      return;
    });
    this.httpClient = httpclient;
  }

  @computedFrom('currentUser')
  get isLoggedIn() {
    return this.currentUser && this.currentUser.access_token && !this.currentUser.expired;
  }

}
