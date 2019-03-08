import { inject } from 'aurelia-framework';
import { SampleService } from 'services/sample-service';

@inject(SampleService)
export class Main {

  constructor(service) {
    this._service = service;
  }

  effacerCacheAutorisations() {
    return this._service.effacerCacheAutorisations('test')
      .then(result => this.result = result);
  }

}
