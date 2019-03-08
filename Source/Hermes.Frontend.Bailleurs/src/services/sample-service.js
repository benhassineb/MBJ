import { inject } from 'aurelia-framework';
import { ConfiguredHttpClient } from 'core/configured-http-client';
import { handleApiError } from 'core/common';

@inject(ConfiguredHttpClient)
export class SampleService {

  /**
   * Create an instance of the class.
   * @param {ConfiguredHttpClient} configuredhttpclient - le client http
   */
  constructor(configuredhttpclient) {
    this._client = configuredhttpclient.httpClient;
  }

  /**
   * Efface le cache des autorisations.
   * @return {Promise} - la réponse http
   */
  effacerCacheAutorisations() {
    const url = '/backoffice/1.0/cache/autorisations';
    return this._client
      .fetch(url, {method: 'DELETE'})
      .then(response => response.json())
      .catch(error => handleApiError(error));
  }

}
