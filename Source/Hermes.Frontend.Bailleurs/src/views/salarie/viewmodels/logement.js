import { inject } from 'aurelia-framework';
import { Auth } from '../auth';

@inject(Auth)
export class Logement {
  constructor(auth) {
    this.auth = auth;
    this.logement = { 'commune': 'Versailles', 'nbrPiece': '1', 'superficie': '21', 'price': '750', 'image': '/images/apt123.jpg', 'points': '98', 'ascenseur': 'Oui', 'gardien': 'Oui', 'parking': 'Non', 'etage': '3' };
  }
}
