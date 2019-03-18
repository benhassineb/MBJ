export class Auth {

  authIsLoggedIn = false;
  get isLoggedIn() { return this.authIsLoggedIn; }
  connect() { this.authIsLoggedIn = true; }
  disconnect() { this.authIsLoggedIn = false; }

}
