import { Router } from 'aurelia-router';
import { inject } from 'aurelia-framework';
import { SampleService } from '../../services/sample-service';
import { ConsulterLogement } from '../../models/consulter-logement';

@inject(SampleService, Router)
export class SuiviPlacement {     
  constructor(service, router) {
    this._service = service;
    this._router = router;
    this.pageDemandes = new ConsulterLogement(this._service, this._router)
  }
}
