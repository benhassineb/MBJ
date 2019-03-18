import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-fetch-client';
import { handleApiError } from 'core/common';


@inject(HttpClient)
export class DetailsDemandeService {

  constructor(http) {

    this.http = http;
  }

  getDetailsDemandeAvecCERFA() {
    return this.http.fetch('/mock/details-demande-avec-CERFA.json')
      .then(response => response.json())
      .catch(error => handleApiError(error));
  }

  getDetailsDemandeSansCERFA() {
    return this.http.fetch('/mock/details-demande-sans-CERFA.json')
      .then(response => response.json())
      .catch(error => handleApiError(error));
  }

}