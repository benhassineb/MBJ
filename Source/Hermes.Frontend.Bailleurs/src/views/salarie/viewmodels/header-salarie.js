import { inject } from 'aurelia-framework';
import { Auth } from '../auth';
import { Connexion } from './connexion';
import { Deconnexion } from './deconnexion';
import { Profil } from './profil';

@inject(Auth, Connexion, Deconnexion, Profil)
export class HeaderSalarie {     
  constructor(auth, connexion, deconnexion, profil) {
    this.auth = auth;
    this.listMenu = [];
    this.listMenu.push({showItem: !auth.isLoggedIn, item: connexion});
    this.listMenu.push({showItem: auth.isLoggedIn, item: deconnexion});
    this.listMenu.push({showItem: auth.isLoggedIn, item: profil});
  }
}
