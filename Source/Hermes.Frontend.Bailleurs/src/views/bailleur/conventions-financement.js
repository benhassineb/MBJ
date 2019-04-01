import { Router } from 'aurelia-router';
import { inject } from 'aurelia-framework';
import { SampleService } from '../../services/sample-service';

@inject(SampleService, Router)
export class ConventionsFinancement {     
  constructor(service, router) {
    this._service = service;
    this._router = router;
  }
  activate() {
    this._service.getConventionFinancementAConsulter().then(res => this.resultats = res);
  }
}
