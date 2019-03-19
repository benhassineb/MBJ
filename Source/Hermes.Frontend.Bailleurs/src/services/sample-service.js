import { inject, NewInstance } from 'aurelia-framework';
import { ConfiguredHttpClient } from 'core/configured-http-client';
import { HttpClient } from 'aurelia-fetch-client';
import { handleApiError } from 'core/common';
import { DemandeAConsulterItem } from '../models/demande-a-consulter-item';
import { DemandeAValiderItem } from '../models/demande-a-valider-item';
import { PrioriteAGererItem } from '../models/priorite-a-gerer-item';

@inject(ConfiguredHttpClient, NewInstance.of(HttpClient))
export class SampleService {

  /**
   * Create an instance of the class.
   * @param {ConfiguredHttpClient} configuredhttpclient - le client http configuré pour la Web API.
   * @param {HttpClient} httpclient - le client http fetch standard.
   */
  constructor(configuredhttpclient, httpclient) {
    this._client = configuredhttpclient.httpClient;
    this._localClient = httpclient;
  }

  /**
   * Efface le cache des autorisations.
   * @return {Promise} - la réponse http
   */
  effacerCacheAutorisations() {
    const url = '/backoffice/1.0/cache/autorisations';
    return this._client
      .fetch(url, { method: 'DELETE' })
      .then(response => response.json())
      .catch(error => handleApiError(error));
  }

  getLogements() {
    return this._localClient
      .fetch('/mock/logements.json')
      .then(response => response.json())
      .catch(error => handleApiError(error));
  }

  getCommunes() {
    return this._localClient
      .fetch('/mock/communes.json')
      .then(response => response.json())
      .catch(error => handleApiError(error));
  }

  getReseauFerre() {
    return this._localClient
      .fetch('/mock/reseau.json')
      .then(response => response.json())
      .catch(error => handleApiError(error));
  }

  getContactPriviligies() {
    return this._localClient
      .fetch('/mock/contactprivilegie.json')
      .then(response => response.json())
      .catch(error => handleApiError(error));
  }

  getDetailsDemandeAvecCERFA() {
    return this._localClient.fetch('/mock/details-demande-avec-CERFA.json')
      .then(response => response.json())
      .catch(error => handleApiError(error));
  }

  getDetailsDemandeSansCERFA() {
    return this._localClient.fetch('/mock/details-demande-sans-CERFA.json')
      .then(response => response.json())
      .catch(error => handleApiError(error));
  }

  getDemandesAConsulter() {
    return this._localClient
      .fetch('/mock/demandes-a-consulter.json')
      .then(response => response.json())
      .then(results => {
        return results.map(v => DemandeAConsulterItem.fromObject(v));
      })
      .catch(error => handleApiError(error));
  }

  getDemandesAValider() {
    return this._localClient
      .fetch('/mock/demandes-a-valider.json')
      .then(response => response.json())
      .then(results => {
        return results.map(v => DemandeAValiderItem.fromObject(v));
      })
      .catch(error => handleApiError(error));
  }

  getPrioritesAGerer() {
    return this._localClient
      .fetch('/mock/priorites-a-gerer.json')
      .then(response => response.json())
      .then(results => {
        return results.map(v => PrioriteAGererItem.fromObject(v));
      })
      .catch(error => handleApiError(error));
  }

  getFiltreEntreprise() {
    return this._localClient
      .fetch('/mock/filtreEntreprise.json')
      .then(response => response.json())
      .catch(error => handleApiError(error));
  }

}
