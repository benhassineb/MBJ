import { inject, customElement, bindable, bindingMode } from 'aurelia-framework';
import { OpenIdConnect } from 'aurelia-open-id-connect';

@inject(OpenIdConnect)
@customElement('connect-button')
export default class {

  @bindable({ defaultBindingMode: bindingMode.oneWay })
  mode = 'login';

  constructor(openIdConnect) {
    this.openIdConnect = openIdConnect;
  }

  get isLoggedIn() {
    return this.user !== null && this.user !== undefined;
  }

  attached() {
    this.openIdConnect.addOrRemoveHandler('addUserUnloaded', () => {
      this.user = null;
    });

    this.openIdConnect.addOrRemoveHandler('addUserLoaded', () => {
      this.openIdConnect.getUser().then(user => {
        this.user = user;
      });
    });

    this.openIdConnect.getUser().then(user => {
      this.user = user;
    });
  }

  login() {
    this.openIdConnect.login();
  }

  logout() {
    this.openIdConnect.logout();
  }

}
